import React, { useState } from 'react';
import { Link, useNavigate } from 'react-router-dom';
import { Menu, X, Car, User, LogOut } from 'lucide-react';
import { motion, AnimatePresence } from 'framer-motion';

const Navbar = () => {
    const [isOpen, setIsOpen] = useState(false);
    const navigate = useNavigate();
    const token = localStorage.getItem('token');
    const user = JSON.parse(localStorage.getItem('user') || '{}');

    const handleLogout = () => {
        localStorage.removeItem('token');
        localStorage.removeItem('user');
        navigate('/login');
    };

    return (
        <nav className="fixed w-full z-50 glass border-b-0 top-0 start-0">
            <div className="max-w-7xl flex flex-wrap items-center justify-between mx-auto p-4">
                <Link to="/" className="flex items-center space-x-3 rtl:space-x-reverse">
                    <Car className="text-primary w-8 h-8" />
                    <span className="self-center text-2xl font-bold whitespace-nowrap text-dark">Rent-A-Ride</span>
                </Link>
                <div className="flex md:order-2 space-x-3 md:space-x-0 rtl:space-x-reverse">
                    {token ? (
                        <div className="flex items-center gap-4">
                            <span className="hidden md:block text-sm text-gray-600">Hi, {user.name || 'User'}</span>
                            <Link to="/dashboard" className="hidden md:flex items-center gap-2 text-sm font-medium hover:text-primary transition-colors">
                                <User size={18} /> Dashboard
                            </Link>
                            <button
                                onClick={handleLogout}
                                className="btn-primary flex items-center gap-2 text-sm"
                            >
                                <LogOut size={16} /> Logout
                            </button>
                        </div>
                    ) : (
                        <Link to="/login" className="btn-primary text-sm">
                            Get Started
                        </Link>
                    )}
                    <button
                        onClick={() => setIsOpen(!isOpen)}
                        className="inline-flex items-center p-2 w-10 h-10 justify-center text-sm text-gray-500 rounded-lg md:hidden hover:bg-gray-100 focus:outline-none focus:ring-2 focus:ring-gray-200"
                    >
                        <span className="sr-only">Open main menu</span>
                        {isOpen ? <X /> : <Menu />}
                    </button>
                </div>
                <div className="items-center justify-between hidden w-full md:flex md:w-auto md:order-1">
                    <ul className="flex flex-col p-4 md:p-0 mt-4 font-medium border border-gray-100 rounded-lg md:space-x-8 rtl:space-x-reverse md:flex-row md:mt-0 md:border-0">
                        <li>
                            <Link to="/" className="block py-2 px-3 text-secondary rounded hover:text-primary md:p-0 transition-colors">Home</Link>
                        </li>
                        <li>
                            <Link to="/browse" className="block py-2 px-3 text-secondary rounded hover:text-primary md:p-0 transition-colors">Browse</Link>
                        </li>
                    </ul>
                </div>
            </div>

            <AnimatePresence>
                {isOpen && (
                    <motion.div
                        initial={{ opacity: 0, height: 0 }}
                        animate={{ opacity: 1, height: 'auto' }}
                        exit={{ opacity: 0, height: 0 }}
                        className="md:hidden glass"
                    >
                        <ul className="flex flex-col p-4 space-y-4 font-medium">
                            <li>
                                <Link to="/" className="block py-2 px-3 text-dark rounded hover:bg-gray-100" onClick={() => setIsOpen(false)}>Home</Link>
                            </li>
                            <li>
                                <Link to="/browse" className="block py-2 px-3 text-dark rounded hover:bg-gray-100" onClick={() => setIsOpen(false)}>Browse</Link>
                            </li>
                            {token && (
                                <li>
                                    <Link to="/dashboard" className="block py-2 px-3 text-dark rounded hover:bg-gray-100" onClick={() => setIsOpen(false)}>Dashboard</Link>
                                </li>
                            )}
                        </ul>
                    </motion.div>
                )}
            </AnimatePresence>
        </nav>
    );
};

export default Navbar;
