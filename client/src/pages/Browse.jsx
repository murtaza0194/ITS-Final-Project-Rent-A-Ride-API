import React, { useEffect, useState } from 'react';
import { vehicleAPI } from '../api';
import Layout from '../components/Layout';
import VehicleCard from '../components/VehicleCard';
import BookingModal from '../components/BookingModal';
import { toast } from 'react-hot-toast';
import { Loader2, Search } from 'lucide-react';

const Browse = () => {
    const [vehicles, setVehicles] = useState([]);
    const [loading, setLoading] = useState(true);
    const [searchTerm, setSearchTerm] = useState('');
    const [selectedVehicle, setSelectedVehicle] = useState(null);
    const [isModalOpen, setIsModalOpen] = useState(false);

    useEffect(() => {
        fetchVehicles();
    }, []);

    const fetchVehicles = async () => {
        try {
            setLoading(true);
            const response = await vehicleAPI.getAll();
            // The API returns paginated data: { items: [], totalCount: ... }
            setVehicles(response.data.data?.items || []);
        } catch (error) {
            console.error("Failed to fetch vehicles", error);
        } finally {
            setLoading(false);
        }
    };

    const handleBook = (vehicle) => {
        const token = localStorage.getItem('token');
        if (!token) {
            toast.error("Please login to book a vehicle");
            return;
        }
        setSelectedVehicle(vehicle);
        setIsModalOpen(true);
    };

    const filteredVehicles = vehicles.filter(v =>
        v.model.toLowerCase().includes(searchTerm.toLowerCase())
    );

    return (
        <Layout>
            <div className="max-w-7xl mx-auto px-4 py-8">
                <div className="flex flex-col md:flex-row md:items-center justify-between gap-4 mb-8">
                    <div>
                        <h1 className="text-3xl font-bold text-dark">Our Fleet</h1>
                        <p className="text-gray-500 mt-1">Choose the perfect ride for your journey</p>
                    </div>

                    <div className="relative">
                        <Search className="absolute left-3 top-1/2 -translate-y-1/2 text-gray-400" size={20} />
                        <input
                            type="text"
                            placeholder="Search by model..."
                            value={searchTerm}
                            onChange={(e) => setSearchTerm(e.target.value)}
                            className="pl-10 pr-4 py-2 rounded-full border border-gray-200 focus:border-primary focus:ring-2 focus:ring-blue-100 outline-none w-full md:w-64 transition-all"
                        />
                    </div>
                </div>

                {loading ? (
                    <div className="flex items-center justify-center h-64">
                        <Loader2 className="w-12 h-12 text-primary animate-spin" />
                    </div>
                ) : (
                    <div className="grid grid-cols-1 sm:grid-cols-2 lg:grid-cols-3 gap-6">
                        {filteredVehicles.map(vehicle => (
                            <VehicleCard key={vehicle.id} vehicle={vehicle} onBook={handleBook} />
                        ))}

                        {filteredVehicles.length === 0 && (
                            <div className="col-span-full text-center py-12 text-gray-400">
                                No vehicles found matching your search.
                            </div>
                        )}
                    </div>
                )}
            </div>

            {selectedVehicle && (
                <BookingModal
                    vehicle={selectedVehicle}
                    isOpen={isModalOpen}
                    onClose={() => setIsModalOpen(false)}
                />
            )}
        </Layout>
    );
};

export default Browse;
