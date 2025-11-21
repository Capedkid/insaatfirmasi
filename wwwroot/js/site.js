// Hero Slider JavaScript
document.addEventListener('DOMContentLoaded', function() {
    const heroCarousel = document.getElementById('heroCarousel');
    
    if (heroCarousel) {
        // Pause carousel on hover
        heroCarousel.addEventListener('mouseenter', function() {
            const carousel = bootstrap.Carousel.getInstance(this);
            if (carousel) {
                carousel.pause();
            }
        });
        
        // Resume carousel when mouse leaves
        heroCarousel.addEventListener('mouseleave', function() {
            const carousel = bootstrap.Carousel.getInstance(this);
            if (carousel) {
                carousel.cycle();
            }
        });
        
        // Add smooth transitions between slides
        heroCarousel.addEventListener('slide.bs.carousel', function(e) {
            const activeSlide = e.target.querySelector('.carousel-item.active');
            const nextSlide = e.relatedTarget;
            
            // Reset animations for next slide
            const nextContent = nextSlide.querySelector('.hero-content');
            if (nextContent) {
                const elements = nextContent.querySelectorAll('.hero-badge, h1, p, .hero-features, .hero-buttons');
                elements.forEach((el, index) => {
                    el.style.animation = 'none';
                    el.offsetHeight; // Trigger reflow
                    el.style.animation = `fadeInUp 0.8s ease-out ${index * 0.2}s both`;
                });
            }
        });
        
        // Add keyboard navigation
        document.addEventListener('keydown', function(e) {
            if (e.key === 'ArrowLeft') {
                const carousel = bootstrap.Carousel.getInstance(heroCarousel);
                if (carousel) {
                    carousel.prev();
                }
            } else if (e.key === 'ArrowRight') {
                const carousel = bootstrap.Carousel.getInstance(heroCarousel);
                if (carousel) {
                    carousel.next();
                }
            }
        });
        
        // Touch/swipe support for mobile
        let startX = 0;
        let endX = 0;
        
        heroCarousel.addEventListener('touchstart', function(e) {
            startX = e.touches[0].clientX;
        });
        
        heroCarousel.addEventListener('touchend', function(e) {
            endX = e.changedTouches[0].clientX;
            handleSwipe();
        });
        
        function handleSwipe() {
            const threshold = 50;
            const diff = startX - endX;
            
            if (Math.abs(diff) > threshold) {
                const carousel = bootstrap.Carousel.getInstance(heroCarousel);
                if (carousel) {
                    if (diff > 0) {
                        carousel.next();
                    } else {
                        carousel.prev();
                    }
                }
            }
        }
    }
    
    // Add intersection observer for animations
    const observerOptions = {
        threshold: 0.1,
        rootMargin: '0px 0px -50px 0px'
    };
    
    const observer = new IntersectionObserver(function(entries) {
        entries.forEach(entry => {
            if (entry.isIntersecting) {
                entry.target.classList.add('animate-in');
            }
        });
    }, observerOptions);
    
    // Observe elements for animation
    const animateElements = document.querySelectorAll('.category-card, .product-card');
    animateElements.forEach(el => {
        observer.observe(el);
    });
    
    // Add hover effects to cards
    const cards = document.querySelectorAll('.category-card, .product-card');
    cards.forEach(card => {
        card.addEventListener('mouseenter', function() {
            this.style.transform = 'translateY(-5px)';
            this.style.transition = 'transform 0.3s ease';
        });
        card.addEventListener('mouseleave', function() {
            this.style.transform = 'translateY(0)';
        });
    });
    
    // Smooth scrolling for anchor links
    document.querySelectorAll('a[href^="#"]').forEach(anchor => {
        anchor.addEventListener('click', function (e) {
            e.preventDefault();
            const target = document.querySelector(this.getAttribute('href'));
            if (target) {
                target.scrollIntoView({
                    behavior: 'smooth'
                });
            }
        });
    });
    
    // Add loading animation
    window.addEventListener('load', function() {
        document.body.classList.add('loaded');
    });
});
