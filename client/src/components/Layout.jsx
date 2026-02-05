import React from 'react';
import Navbar from './Navbar';
import { Toaster } from 'react-hot-toast';

const Layout = ({ children }) => {
    return (
        <div className="min-h-screen bg-light">
            <Navbar />
            <main className="pt-20 min-h-[calc(100vh-80px)]">
                {children}
            </main>
            <Toaster position="top-right" />
        </div>
    );
};

export default Layout;
