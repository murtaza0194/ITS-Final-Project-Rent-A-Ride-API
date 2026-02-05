import React from 'react';
import { motion } from 'framer-motion';
import { Link } from 'react-router-dom';
import { ArrowRight, ShieldCheck, Zap, Heart } from 'lucide-react';

const Hero = () => {
    return (
        <section className="relative overflow-hidden pt-16 pb-32">
            {/* Background Elements */}
            <div className="absolute top-0 left-0 w-full h-full -z-10 bg-linear-to-br from-blue-50 to-white"></div>
            <div className="absolute top-0 right-0 w-1/3 h-full bg-linear-to-l from-blue-100/50 to-transparent rounded-l-full blur-3xl"></div>

            <div className="max-w-7xl mx-auto px-4 flex flex-col md:flex-row items-center">
                {/* Text Content */}
                <motion.div
                    initial={{ opacity: 0, x: -50 }}
                    animate={{ opacity: 1, x: 0 }}
                    transition={{ duration: 0.8 }}
                    className="md:w-1/2 text-center md:text-left z-10"
                >
                    <div className="inline-block px-4 py-1.5 mb-6 text-sm font-semibold text-primary bg-blue-100 rounded-full">
                        Premium Car Rental Service
                    </div>
                    <h1 className="text-5xl md:text-6xl font-extrabold text-dark leading-tight mb-6">
                        Drive Your <span className="text-primary">Dream</span> today.
                    </h1>
                    <p className="text-lg text-gray-600 mb-8 max-w-lg mx-auto md:mx-0">
                        Experience the thrill of the open road with our premium fleet. Flexible booking, transparent pricing, and 24/7 support.
                    </p>
                    <div className="flex flex-col sm:flex-row gap-4 justify-center md:justify-start">
                        <Link to="/browse" className="btn-primary flex items-center justify-center gap-2 text-lg">
                            Browse Fleet <ArrowRight size={20} />
                        </Link>
                        {!localStorage.getItem('token') && (
                            <Link to="/register" className="btn-outline flex items-center justify-center text-lg">
                                Sign Up Now
                            </Link>
                        )}
                    </div>

                    <div className="mt-12 flex items-center justify-center md:justify-start gap-8 text-secondary">
                        <div className="flex items-center gap-2">
                            <ShieldCheck className="text-primary" /> <span className="font-medium">Secure</span>
                        </div>
                        <div className="flex items-center gap-2">
                            <Zap className="text-accent" /> <span className="font-medium">Fast</span>
                        </div>
                        <div className="flex items-center gap-2">
                            <Heart className="text-red-500" /> <span className="font-medium">Trusted</span>
                        </div>
                    </div>
                </motion.div>

                {/* Visual/Image */}
                <motion.div
                    initial={{ opacity: 0, x: 50 }}
                    animate={{ opacity: 1, x: 0 }}
                    transition={{ duration: 0.8, delay: 0.2 }}
                    className="md:w-1/2 mt-12 md:mt-0 relative"
                >
                    {/* Abstract representation of a car or a nice placeholder */}
                    <div className="relative z-10 glass p-4 rounded-3xl shadow-2xl transform rotate-1 hover:rotate-0 transition-transform duration-500">
                        <img
                            src="https://images.unsplash.com/photo-1492144534655-ae79c964c9d7?auto=format&fit=crop&q=80&w=1000"
                            alt="Luxury Car"
                            className="rounded-2xl w-full h-auto object-cover"
                        />
                        <div className="absolute bottom-8 left-8 glass px-6 py-3 rounded-xl flex items-center gap-4 animate-bounce">
                            <div className="w-12 h-12 bg-primary rounded-full flex items-center justify-center text-white font-bold">
                                $99
                            </div>
                            <div>
                                <p className="text-xs text-gray-500 font-semibold uppercase">Daily Rate</p>
                                <p className="font-bold text-dark">Premium Class</p>
                            </div>
                        </div>
                    </div>
                </motion.div>
            </div >
        </section >
    );
};

export default Hero;
