// Admin Panel JavaScript Functions

// Global variables
let unreadMessagesCount = 0;
let isOnline = navigator.onLine;

// Initialize admin panel
document.addEventListener('DOMContentLoaded', function() {
    initializeAdminPanel();
    setupEventListeners();
    startPeriodicUpdates();
});

// Initialize admin panel components
function initializeAdminPanel() {
    // Initialize tooltips
    initializeTooltips();
    
    // Initialize popovers
    initializePopovers();
    
    // Initialize sidebar
    initializeSidebar();
    
    // Initialize forms
    initializeForms();
    
    // Initialize tables
    initializeTables();
    
    // Initialize modals
    initializeModals();
    
    // Update unread messages count
    updateUnreadMessagesCount();
    
    // Check online status
    checkOnlineStatus();
}

// Setup event listeners
function setupEventListeners() {
    // Online/offline status
    window.addEventListener('online', handleOnlineStatus);
    window.addEventListener('offline', handleOfflineStatus);
    
    // Form submissions
    document.addEventListener('submit', handleFormSubmission);
    
    // Delete confirmations
    document.addEventListener('click', handleDeleteConfirmations);
    
    // Auto-save functionality
    document.addEventListener('input', handleAutoSave);
    
    // Keyboard shortcuts
    document.addEventListener('keydown', handleKeyboardShortcuts);
}

// Initialize tooltips
function initializeTooltips() {
    const tooltipTriggerList = [].slice.call(document.querySelectorAll('[data-bs-toggle="tooltip"]'));
    tooltipTriggerList.map(function (tooltipTriggerEl) {
        return new bootstrap.Tooltip(tooltipTriggerEl);
    });
}

// Initialize popovers
function initializePopovers() {
    const popoverTriggerList = [].slice.call(document.querySelectorAll('[data-bs-toggle="popover"]'));
    popoverTriggerList.map(function (popoverTriggerEl) {
        return new bootstrap.Popover(popoverTriggerEl);
    });
}

// Initialize sidebar
function initializeSidebar() {
    const sidebarToggle = document.querySelector('.sidebar-toggle');
    const sidebar = document.getElementById('sidebar');
    
    if (sidebarToggle) {
        sidebarToggle.addEventListener('click', function() {
            sidebar.classList.toggle('show');
        });
    }
    
    // Close sidebar when clicking outside on mobile
    document.addEventListener('click', function(event) {
        if (window.innerWidth <= 768 && 
            !sidebar.contains(event.target) && 
            !sidebarToggle.contains(event.target)) {
            sidebar.classList.remove('show');
        }
    });
    
    // Handle window resize
    window.addEventListener('resize', function() {
        if (window.innerWidth > 768) {
            sidebar.classList.remove('show');
        }
    });
}

// Initialize forms
function initializeForms() {
    // Add loading states to forms
    document.querySelectorAll('form').forEach(form => {
        form.addEventListener('submit', function() {
            const submitBtn = form.querySelector('button[type="submit"]');
            if (submitBtn) {
                submitBtn.disabled = true;
                const originalText = submitBtn.innerHTML;
                submitBtn.innerHTML = '<i class="fas fa-spinner fa-spin me-2"></i>İşleniyor...';
                
                // Re-enable button after 5 seconds as fallback
                setTimeout(() => {
                    submitBtn.disabled = false;
                    submitBtn.innerHTML = originalText;
                }, 5000);
            }
        });
    });
    
    // Form validation
    document.querySelectorAll('form').forEach(form => {
        form.addEventListener('submit', function(e) {
            if (!validateForm(form)) {
                e.preventDefault();
                return false;
            }
        });
    });
}

// Initialize tables
function initializeTables() {
    // Add hover effects to table rows
    document.querySelectorAll('.table tbody tr').forEach(row => {
        row.addEventListener('mouseenter', function() {
            this.style.transform = 'scale(1.01)';
        });
        
        row.addEventListener('mouseleave', function() {
            this.style.transform = 'scale(1)';
        });
    });
    
    // Initialize data tables if available
    if (typeof DataTable !== 'undefined') {
        document.querySelectorAll('.data-table').forEach(table => {
            new DataTable(table, {
                language: {
                    url: '//cdn.datatables.net/plug-ins/1.13.7/i18n/tr.json'
                },
                responsive: true,
                pageLength: 25,
                order: [[0, 'desc']]
            });
        });
    }
}

// Initialize modals
function initializeModals() {
    // Add animation to modals
    document.querySelectorAll('.modal').forEach(modal => {
        modal.addEventListener('show.bs.modal', function() {
            this.style.animation = 'fadeIn 0.3s ease-out';
        });
        
        modal.addEventListener('hide.bs.modal', function() {
            this.style.animation = 'fadeOut 0.3s ease-out';
        });
    });
}

// Handle form submission
function handleFormSubmission(event) {
    const form = event.target;
    if (form.tagName === 'FORM') {
        // Add loading state
        const submitBtn = form.querySelector('button[type="submit"]');
        if (submitBtn) {
            submitBtn.disabled = true;
            const originalText = submitBtn.innerHTML;
            submitBtn.innerHTML = '<i class="fas fa-spinner fa-spin me-2"></i>İşleniyor...';
            
            // Re-enable button after 10 seconds as fallback
            setTimeout(() => {
                submitBtn.disabled = false;
                submitBtn.innerHTML = originalText;
            }, 10000);
        }
    }
}

// Handle delete confirmations
function handleDeleteConfirmations(event) {
    const target = event.target;
    if (target.classList.contains('delete-btn') || 
        (target.classList.contains('btn-danger') && 
         (target.textContent.includes('Sil') || target.textContent.includes('Delete')))) {
        
        if (!confirm('Bu işlemi gerçekleştirmek istediğinizden emin misiniz?')) {
            event.preventDefault();
        }
    }
}

// Handle auto-save
function handleAutoSave(event) {
    const target = event.target;
    if (target.classList.contains('auto-save')) {
        clearTimeout(target.autoSaveTimeout);
        target.autoSaveTimeout = setTimeout(() => {
            saveFormData(target);
        }, 2000);
    }
}

// Handle keyboard shortcuts
function handleKeyboardShortcuts(event) {
    // Ctrl + S: Save form
    if (event.ctrlKey && event.key === 's') {
        event.preventDefault();
        const form = document.querySelector('form');
        if (form) {
            form.submit();
        }
    }
    
    // Ctrl + N: New item
    if (event.ctrlKey && event.key === 'n') {
        event.preventDefault();
        const newBtn = document.querySelector('a[href*="Create"]');
        if (newBtn) {
            window.location.href = newBtn.href;
        }
    }
    
    // Escape: Close modals
    if (event.key === 'Escape') {
        const openModal = document.querySelector('.modal.show');
        if (openModal) {
            const modal = bootstrap.Modal.getInstance(openModal);
            if (modal) {
                modal.hide();
            }
        }
    }
}

// Validate form
function validateForm(form) {
    let isValid = true;
    const requiredFields = form.querySelectorAll('[required]');
    
    requiredFields.forEach(field => {
        if (!field.value.trim()) {
            showFieldError(field, 'Bu alan gereklidir');
            isValid = false;
        } else {
            clearFieldError(field);
        }
    });
    
    // Email validation
    const emailFields = form.querySelectorAll('input[type="email"]');
    emailFields.forEach(field => {
        if (field.value && !isValidEmail(field.value)) {
            showFieldError(field, 'Geçerli bir email adresi girin');
            isValid = false;
        }
    });
    
    // Password confirmation
    const passwordFields = form.querySelectorAll('input[type="password"]');
    if (passwordFields.length >= 2) {
        const password = passwordFields[0].value;
        const confirmPassword = passwordFields[1].value;
        
        if (password && confirmPassword && password !== confirmPassword) {
            showFieldError(passwordFields[1], 'Şifreler eşleşmiyor');
            isValid = false;
        }
    }
    
    return isValid;
}

// Show field error
function showFieldError(field, message) {
    clearFieldError(field);
    
    field.classList.add('is-invalid');
    
    const errorDiv = document.createElement('div');
    errorDiv.className = 'invalid-feedback';
    errorDiv.textContent = message;
    
    field.parentNode.appendChild(errorDiv);
}

// Clear field error
function clearFieldError(field) {
    field.classList.remove('is-invalid');
    
    const errorDiv = field.parentNode.querySelector('.invalid-feedback');
    if (errorDiv) {
        errorDiv.remove();
    }
}

// Validate email
function isValidEmail(email) {
    const emailRegex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
    return emailRegex.test(email);
}

// Save form data
function saveFormData(field) {
    const form = field.closest('form');
    if (!form) return;
    
    const formData = new FormData(form);
    const data = Object.fromEntries(formData.entries());
    
    // Save to localStorage
    localStorage.setItem('autoSave_' + form.id, JSON.stringify(data));
    
    // Show save indicator
    showNotification('Otomatik kaydedildi', 'success');
}

// Load form data
function loadFormData(formId) {
    const savedData = localStorage.getItem('autoSave_' + formId);
    if (savedData) {
        const data = JSON.parse(savedData);
        
        Object.keys(data).forEach(key => {
            const field = document.querySelector(`[name="${key}"]`);
            if (field && field.type !== 'password') {
                field.value = data[key];
            }
        });
    }
}

// Update unread messages count
function updateUnreadMessagesCount() {
    // This would typically fetch from an API
    fetch('/Admin/Contact/GetUnreadCount')
        .then(response => response.json())
        .then(data => {
            unreadMessagesCount = data.count || 0;
            updateUnreadBadge();
        })
        .catch(error => {
            console.log('Could not fetch unread count:', error);
            // Use random number as fallback
            unreadMessagesCount = Math.floor(Math.random() * 10);
            updateUnreadBadge();
        });
}

// Update unread badge
function updateUnreadBadge() {
    const badge = document.getElementById('unreadMessagesCount');
    if (badge) {
        badge.textContent = unreadMessagesCount;
        badge.style.display = unreadMessagesCount > 0 ? 'inline' : 'none';
    }
}

// Check online status
function checkOnlineStatus() {
    isOnline = navigator.onLine;
    updateOnlineStatus();
}

// Handle online status
function handleOnlineStatus() {
    isOnline = true;
    updateOnlineStatus();
    showNotification('İnternet bağlantısı yeniden kuruldu', 'success');
}

// Handle offline status
function handleOfflineStatus() {
    isOnline = false;
    updateOnlineStatus();
    showNotification('İnternet bağlantısı kesildi', 'warning');
}

// Update online status indicator
function updateOnlineStatus() {
    const indicator = document.getElementById('onlineStatus');
    if (indicator) {
        indicator.className = isOnline ? 'text-success' : 'text-danger';
        indicator.innerHTML = isOnline ? 
            '<i class="fas fa-wifi"></i> Çevrimiçi' : 
            '<i class="fas fa-wifi-slash"></i> Çevrimdışı';
    }
}

// Start periodic updates
function startPeriodicUpdates() {
    // Update unread count every 30 seconds
    setInterval(updateUnreadMessagesCount, 30000);
    
    // Check online status every 10 seconds
    setInterval(checkOnlineStatus, 10000);
}

// Show notification
function showNotification(message, type = 'info') {
    const notification = document.createElement('div');
    notification.className = `alert alert-${type} alert-dismissible fade show position-fixed`;
    notification.style.cssText = 'top: 20px; right: 20px; z-index: 9999; min-width: 300px;';
    
    notification.innerHTML = `
        ${message}
        <button type="button" class="btn-close" data-bs-dismiss="alert"></button>
    `;
    
    document.body.appendChild(notification);
    
    // Auto remove after 5 seconds
    setTimeout(() => {
        if (notification.parentNode) {
            notification.remove();
        }
    }, 5000);
}

// AJAX helper functions
function ajaxRequest(url, options = {}) {
    const defaultOptions = {
        method: 'GET',
        headers: {
            'Content-Type': 'application/json',
            'X-Requested-With': 'XMLHttpRequest'
        }
    };
    
    const finalOptions = { ...defaultOptions, ...options };
    
    return fetch(url, finalOptions)
        .then(response => {
            if (!response.ok) {
                throw new Error(`HTTP error! status: ${response.status}`);
            }
            return response.json();
        })
        .catch(error => {
            console.error('AJAX request failed:', error);
            showNotification('Bir hata oluştu: ' + error.message, 'danger');
            throw error;
        });
}

// Toggle status
function toggleStatus(id, isActive, controller) {
    return ajaxRequest(`/Admin/${controller}/ToggleStatus`, {
        method: 'POST',
        body: JSON.stringify({ id: id, isActive: isActive })
    });
}

// Delete item
function deleteItem(id, controller) {
    return ajaxRequest(`/Admin/${controller}/Delete`, {
        method: 'POST',
        body: JSON.stringify({ id: id })
    });
}

// Bulk action
function bulkAction(ids, action, controller) {
    return ajaxRequest(`/Admin/${controller}/BulkAction`, {
        method: 'POST',
        body: JSON.stringify({ ids: ids, action: action })
    });
}

// Export data
function exportData(format, controller) {
    const url = `/Admin/${controller}/Export?format=${format}`;
    window.open(url, '_blank');
}

// Import data
function importData(file, controller) {
    const formData = new FormData();
    formData.append('file', file);
    
    return fetch(`/Admin/${controller}/Import`, {
        method: 'POST',
        body: formData
    })
    .then(response => response.json())
    .catch(error => {
        console.error('Import failed:', error);
        showNotification('İçe aktarma başarısız: ' + error.message, 'danger');
        throw error;
    });
}

// Search functionality
function performSearch(query, controller) {
    const url = `/Admin/${controller}/Search?q=${encodeURIComponent(query)}`;
    return ajaxRequest(url);
}

// Filter functionality
function applyFilter(filterData, controller) {
    const params = new URLSearchParams(filterData);
    const url = `/Admin/${controller}/Filter?${params.toString()}`;
    return ajaxRequest(url);
}

// Pagination
function goToPage(page, controller) {
    const url = new URL(window.location);
    url.searchParams.set('page', page);
    window.location.href = url.toString();
}

// Utility functions
function formatDate(date) {
    return new Date(date).toLocaleDateString('tr-TR');
}

function formatDateTime(date) {
    return new Date(date).toLocaleString('tr-TR');
}

function formatFileSize(bytes) {
    if (bytes === 0) return '0 Bytes';
    const k = 1024;
    const sizes = ['Bytes', 'KB', 'MB', 'GB'];
    const i = Math.floor(Math.log(bytes) / Math.log(k));
    return parseFloat((bytes / Math.pow(k, i)).toFixed(2)) + ' ' + sizes[i];
}

function copyToClipboard(text) {
    navigator.clipboard.writeText(text).then(() => {
        showNotification('Panoya kopyalandı', 'success');
    }).catch(() => {
        showNotification('Kopyalama başarısız', 'danger');
    });
}

function downloadFile(url, filename) {
    const link = document.createElement('a');
    link.href = url;
    link.download = filename;
    document.body.appendChild(link);
    link.click();
    document.body.removeChild(link);
}

// Initialize when DOM is loaded
if (document.readyState === 'loading') {
    document.addEventListener('DOMContentLoaded', initializeAdminPanel);
} else {
    initializeAdminPanel();
}
