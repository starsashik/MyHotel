import React, { ReactNode } from 'react';
import '../styles/index.css';
import Header from "../../Components/Header.tsx";
import Footer from "../../Components/Footer.tsx";

interface LayoutProps {
    children: ReactNode;
}

export const Layout: React.FC<LayoutProps> = ({ children }) => {
    return (
        <div className="layout">
            <Header />
                <main className="layout__main">{children}</main>
            <Footer />
        </div>
    );
};
