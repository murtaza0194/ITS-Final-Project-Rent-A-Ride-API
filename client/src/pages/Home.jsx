import React from 'react';
import Hero from '../components/Hero';
import Layout from '../components/Layout';

const Home = () => {
    return (
        <Layout>
            <Hero />
            {/* Additional sections like "How it works" or "Featured" can be added here */}
        </Layout>
    );
};

export default Home;
