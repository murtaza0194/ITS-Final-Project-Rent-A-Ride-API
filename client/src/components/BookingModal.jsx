import React, { useState } from 'react';
import { motion, AnimatePresence } from 'framer-motion';
import { X, Calendar, Loader2 } from 'lucide-react';
import { rentalAPI } from '../api';
import { toast } from 'react-hot-toast';

const BookingModal = ({ vehicle, isOpen, onClose }) => {
    const [startDate, setStartDate] = useState('');
    const [endDate, setEndDate] = useState('');
    const [loading, setLoading] = useState(false);

    const handleBooking = async (e) => {
        e.preventDefault();
        setLoading(true);
        try {
            // In a real app, user ID generally comes from the token on the backend (via `User.Identity.Name`), 
            // but our simplistic API might expect it in the body if we didn't implement fully implicit User ID extraction yet.
            // Looking at RentalService.BookRentalAsync(int userId...), it takes userId.
            // The controller `RentalsController.Create` should extract it from claims.
            // Assuming specific payload: { vehicleId, startDate, endDate, amenityIds: [] }

            const user = JSON.parse(localStorage.getItem('user') || '{}');

            if (!user.id) {
                toast.error("User session invalid. Please login again.");
                return;
            }

            await rentalAPI.book({
                userId: parseInt(user.id), // Ensure integer
                vehicleId: vehicle.id,
                startDate,
                endDate,
                amenityIds: []
            });

            toast.success('Booking Successful! 🎉');
            onClose();
        } catch (error) {
            console.error(error);
            toast.error(error.response?.data?.message || 'Booking failed');
        } finally {
            setLoading(false);
        }
    };

    if (!isOpen) return null;

    return (
        <AnimatePresence>
            <div className="fixed inset-0 z-50 flex items-center justify-center p-4 bg-black/50 backdrop-blur-sm">
                <motion.div
                    initial={{ opacity: 0, scale: 0.9 }}
                    animate={{ opacity: 1, scale: 1 }}
                    exit={{ opacity: 0, scale: 0.9 }}
                    className="bg-white rounded-2xl w-full max-w-md overflow-hidden shadow-2xl"
                >
                    <div className="p-6 border-b border-gray-100 flex justify-between items-center bg-gray-50">
                        <div>
                            <h3 className="text-xl font-bold text-dark">Book {vehicle.model}</h3>
                            <p className="text-sm text-gray-500">${vehicle.dailyPrice}/day</p>
                        </div>
                        <button onClick={onClose} className="p-2 hover:bg-gray-200 rounded-full transition-colors">
                            <X size={20} />
                        </button>
                    </div>

                    <form onSubmit={handleBooking} className="p-6 space-y-4">
                        <div>
                            <label className="block text-sm font-medium text-gray-700 mb-2">Start Date</label>
                            <input
                                type="date"
                                required
                                min={new Date().toISOString().split('T')[0]}
                                value={startDate}
                                onChange={(e) => setStartDate(e.target.value)}
                                className="w-full px-4 py-2 rounded-lg border border-gray-200 focus:ring-2 focus:ring-primary/20 outline-none"
                            />
                        </div>
                        <div>
                            <label className="block text-sm font-medium text-gray-700 mb-2">End Date</label>
                            <input
                                type="date"
                                required
                                min={startDate || new Date().toISOString().split('T')[0]}
                                value={endDate}
                                onChange={(e) => setEndDate(e.target.value)}
                                className="w-full px-4 py-2 rounded-lg border border-gray-200 focus:ring-2 focus:ring-primary/20 outline-none"
                            />
                        </div>

                        <div className="pt-4">
                            <button
                                type="submit"
                                disabled={loading}
                                className="w-full btn-primary py-3 flex justify-center items-center gap-2"
                            >
                                {loading ? <Loader2 className="animate-spin" /> : 'Confirm Booking'}
                            </button>
                        </div>
                    </form>
                </motion.div>
            </div>
        </AnimatePresence>
    );
};

export default BookingModal;
