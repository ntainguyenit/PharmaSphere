/**
 * PharmaSphere Analytics & Reporting JS
 * Handles tracking and client-side data visualization.
 */

class PharmaAnalytics {
    constructor() {
        this.startTime = Date.now();
        this.interactions = [];
        this.trackPageView();
    }

    trackPageView() {
        const path = window.location.pathname;
        console.log(`[Analytics] Page View: ${path}`);
        this.logEvent('page_view', { path });
    }

    logEvent(name, data) {
        const event = {
            name,
            data,
            timestamp: new Date().toISOString()
        };
        this.interactions.push(event);
        
        // In a real app, send this to the server periodically
        if (this.interactions.length > 5) {
            this.flush();
        }
    }

    flush() {
        console.log('[Analytics] Flushing events:', this.interactions);
        this.interactions = [];
    }

    trackProductClick(productId, productName) {
        this.logEvent('product_click', { productId, productName });
    }

    trackCheckoutStarted(orderValue) {
        this.logEvent('checkout_start', { value: orderValue });
    }
}

window.pharmaAnalytics = new PharmaAnalytics();
