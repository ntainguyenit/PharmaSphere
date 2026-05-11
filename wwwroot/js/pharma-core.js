/**
 * PharmaSphere Core JavaScript Library
 * Provides advanced UI interactions, cart management, and real-time updates.
 */

const PharmaCore = (() => {
    'use strict';

    // Private state
    let _cartCount = 0;
    const _endpoints = {
        cart: '/Cart/GetSummary',
        interaction: '/Product/CheckInteraction',
        recommendation: '/Product/GetRelated'
    };

    // Initialize core features
    const init = () => {
        console.log('PharmaSphere Core Initialized');
        _bindEvents();
        _updateCartUI();
    };

    const _bindEvents = () => {
        // Debounced search
        const searchInput = document.getElementById('global-search');
        if (searchInput) {
            searchInput.addEventListener('input', _debounce(handleSearch, 300));
        }

        // Quick add to cart
        document.querySelectorAll('.btn-quick-add').forEach(btn => {
            btn.addEventListener('click', async (e) => {
                const productId = e.target.dataset.productId;
                await addToCart(productId);
            });
        });
    };

    const handleSearch = async (e) => {
        const query = e.target.value;
        if (query.length < 2) return;
        
        console.log(`Searching for: ${query}`);
        // Logic to show live search results
    };

    const addToCart = async (productId) => {
        try {
            const response = await fetch('/Cart/Add', {
                method: 'POST',
                headers: { 'Content-Type': 'application/json' },
                body: JSON.stringify({ productId, quantity: 1 })
            });

            if (response.ok) {
                _cartCount++;
                _updateCartUI();
                _showNotification('Sản phẩm đã được thêm vào giỏ hàng!', 'success');
            }
        } catch (error) {
            console.error('Error adding to cart:', error);
        }
    };

    const _updateCartUI = () => {
        const cartBadge = document.querySelector('.cart-badge');
        if (cartBadge) cartBadge.textContent = _cartCount;
    };

    const _showNotification = (message, type) => {
        // Professional toast notification logic
        const toast = document.createElement('div');
        toast.className = `pharma-toast toast-${type}`;
        toast.textContent = message;
        document.body.appendChild(toast);
        setTimeout(() => toast.remove(), 3000);
    };

    const _debounce = (func, delay) => {
        let timeout;
        return (...args) => {
            clearTimeout(timeout);
            timeout = setTimeout(() => func.apply(this, args), delay);
        };
    };

    return {
        init,
        addToCart,
        getCartCount: () => _cartCount
    };
})();

document.addEventListener('DOMContentLoaded', PharmaCore.init);
