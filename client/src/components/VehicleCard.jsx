import React from 'react';
import { motion } from 'framer-motion';
import { Fuel, Gauge, Calendar, DollarSign } from 'lucide-react';

const getVehicleImage = (model) => {
    // Map specific models to imagin.studio params or specific high-quality fallback URLs
    // Using imagin.studio for consistent, accurate car renders

    const baseUrl = 'https://cdn.imagin.studio/getimage';
    const customer = 'hrn'; // Common demo key, if it fails fallback will trigger

    let make = '';
    let modelFamily = '';

    if (model.includes('Toyota Corolla')) { make = 'toyota'; modelFamily = 'corolla'; }
    else if (model.includes('Hyundai Tucson')) { make = 'hyundai'; modelFamily = 'tucson'; }
    else if (model.includes('Mercedes')) { make = 'mercedes'; modelFamily = 'c-class'; }
    else if (model.includes('Kia Picanto')) { make = 'kia'; modelFamily = 'picanto'; }
    else if (model.includes('Ferrari')) { make = 'ferrari'; modelFamily = '488'; }
    else if (model.includes('Lamborghini')) { make = 'lamborghini'; modelFamily = 'huracan'; }
    else if (model.includes('Porsche')) { make = 'porsche'; modelFamily = '911'; }
    else if (model.includes('BMW')) { make = 'bmw'; modelFamily = 'm4'; }
    else { return 'https://images.unsplash.com/photo-1568605117036-5fe5e7bab0b7?auto=format&fit=crop&q=80&w=1000'; }

    return `${baseUrl}?customer=${customer}&make=${make}&modelFamily=${modelFamily}&zoomType=fullscreen&zoomLevel=30`;
};

const VehicleCard = ({ vehicle, onBook }) => {
    return (
        <motion.div
            initial={{ opacity: 0, scale: 0.95 }}
            whileInView={{ opacity: 1, scale: 1 }}
            viewport={{ once: true }}
            className="bg-white rounded-2xl shadow-lg overflow-hidden border border-gray-100 hover:shadow-2xl transition-shadow duration-300 group"
        >
            <div className="relative h-48 overflow-hidden bg-gray-100">
                {/* Image Mapping Logic */}
                <img
                    src={getVehicleImage(vehicle.model)}
                    onError={(e) => e.target.src = "https://images.unsplash.com/photo-1568605117036-5fe5e7bab0b7?auto=format&fit=crop&q=80&w=1000"}
                    alt={vehicle.model}
                    className="w-full h-full object-cover group-hover:scale-110 transition-transform duration-500"
                />
                <div className="absolute top-3 right-3 bg-white/90 backdrop-blur text-xs font-bold px-3 py-1 rounded-full uppercase tracking-wider">
                    {vehicle.vehicleType?.name || 'Standard'}
                </div>
            </div>

            <div className="p-5">
                <h3 className="text-xl font-bold text-dark mb-1">{vehicle.model}</h3>
                <p className="text-gray-500 text-sm mb-4">{vehicle.year} • {vehicle.licensePlate}</p>

                <div className="grid grid-cols-2 gap-3 mb-4 text-sm text-gray-600">
                    <div className="flex items-center gap-2">
                        <Fuel size={16} className="text-primary" /> <span>Petrol</span>
                    </div>
                    <div className="flex items-center gap-2">
                        <Gauge size={16} className="text-primary" /> <span>Auto</span>
                    </div>
                </div>

                <div className="flex items-center justify-between pt-4 border-t border-gray-100">
                    <div>
                        <span className="text-2xl font-bold text-dark">${vehicle.dailyPrice}</span>
                        <span className="text-gray-400 text-xs ml-1">/ day</span>
                    </div>

                    <button
                        onClick={() => onBook(vehicle)}
                        className="btn-primary py-2 px-5 text-sm"
                    >
                        Rent Now
                    </button>
                </div>
            </div>
        </motion.div>
    );
};

export default VehicleCard;
