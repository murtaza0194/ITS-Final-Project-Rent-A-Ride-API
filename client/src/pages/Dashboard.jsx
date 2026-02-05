import React, { useEffect, useState } from 'react';
import { rentalAPI } from '../api';
import Layout from '../components/Layout';
import { Loader2, Calendar, Car, AlertCircle } from 'lucide-react';
import { format } from 'date-fns';

const Dashboard = () => {
    const [rentals, setRentals] = useState([]);
    const [loading, setLoading] = useState(true);

    useEffect(() => {
        fetchHistory();
    }, []);

    const fetchHistory = async () => {
        try {
            const user = JSON.parse(localStorage.getItem('user') || '{}');
            if (!user.id) return; // Or redirect/show message

            const response = await rentalAPI.getMyHistory(user.id);
            // ServiceResult.Data contains the list
            setRentals(response.data.data || []);
        } catch (error) {
            console.error(error);
        } finally {
            setLoading(false);
        }
    };

    const getStatusColor = (status) => {
        switch (status) {
            case 'Active': return 'bg-green-100 text-green-700';
            case 'Completed': return 'bg-gray-100 text-gray-700';
            case 'Cancelled': return 'bg-red-100 text-red-700';
            default: return 'bg-blue-100 text-blue-700';
        }
    };

    return (
        <Layout>
            <div className="max-w-7xl mx-auto px-4 py-8">
                <h1 className="text-3xl font-bold text-dark mb-8">My Rentals</h1>

                {loading ? (
                    <div className="flex justify-center py-12">
                        <Loader2 className="animate-spin text-primary w-8 h-8" />
                    </div>
                ) : rentals.length === 0 ? (
                    <div className="text-center py-12 bg-white rounded-2xl border border-gray-100">
                        <div className="bg-gray-50 w-16 h-16 rounded-full flex items-center justify-center mx-auto mb-4">
                            <Car className="text-gray-400" size={32} />
                        </div>
                        <h3 className="text-lg font-medium text-gray-900">No rentals yet</h3>
                        <p className="text-gray-500 mt-1">Book your first car today!</p>
                    </div>
                ) : (
                    <div className="grid gap-4">
                        {rentals.map(rental => (
                            <div key={rental.id} className="bg-white p-6 rounded-xl shadow-sm border border-gray-100 flex flex-col md:flex-row items-start md:items-center justify-between gap-4">
                                <div className="flex items-center gap-4">
                                    <div className="bg-blue-50 p-3 rounded-lg">
                                        <Car className="text-primary" size={24} />
                                    </div>
                                    <div>
                                        <h3 className="font-bold text-dark text-lg">{rental.vehicle?.model || 'Unknown Car'}</h3>
                                        <p className="text-sm text-gray-500">{rental.vehicle?.licensePlate}</p>
                                    </div>
                                </div>

                                <div className="flex flex-col md:flex-row gap-4 md:gap-8 flex-1 md:justify-center">
                                    <div className="flex items-center gap-2 text-sm text-gray-600">
                                        <Calendar size={16} />
                                        <span>{format(new Date(rental.startDate), 'MMM dd, yyyy')}</span>
                                        <span className="text-gray-300">→</span>
                                        <span>{format(new Date(rental.endDate), 'MMM dd, yyyy')}</span>
                                    </div>

                                    <div className="font-semibold text-dark">
                                        ${rental.totalPrice.toFixed(2)}
                                    </div>
                                </div>

                                <div>
                                    <span className={`px-3 py-1 rounded-full text-xs font-bold uppercase ${getStatusColor(rental.status)}`}>
                                        {rental.status}
                                    </span>
                                </div>
                            </div>
                        ))}
                    </div>
                )}
            </div>
        </Layout>
    );
};

export default Dashboard;
