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

    // HEADER SEARCH
    const searchButton = document.getElementById('headerSearchButton');
    const searchOverlay = document.getElementById('headerSearchOverlay');
    const searchInput = document.getElementById('headerSearchInput');
    const searchClose = document.getElementById('headerSearchClose');
    const searchResults = document.getElementById('headerSearchResults');

    let searchTimeout;

    function openSearch() {
        if (!searchOverlay || !searchInput) return;
        searchOverlay.classList.add('open');
        searchOverlay.setAttribute('aria-hidden', 'false');
        setTimeout(() => searchInput.focus(), 50);
    }

    function closeSearch() {
        if (!searchOverlay || !searchInput) return;
        searchOverlay.classList.remove('open');
        searchOverlay.setAttribute('aria-hidden', 'true');
        searchInput.value = '';
        if (searchResults) {
            searchResults.innerHTML = '<div class=\"header-search-empty\">Arama yapmak için en az 2 karakter yazın.</div>';
        }
    }

    if (searchButton && searchOverlay && searchInput && searchResults) {
        searchButton.addEventListener('click', openSearch);
        if (searchClose) {
            searchClose.addEventListener('click', closeSearch);
        }
        const backdrop = searchOverlay.querySelector('.header-search-backdrop');
        if (backdrop) {
            backdrop.addEventListener('click', closeSearch);
        }

        document.addEventListener('keydown', function(e) {
            if (e.key === 'Escape' && searchOverlay.classList.contains('open')) {
                closeSearch();
            }
        });

        searchInput.addEventListener('input', function() {
            const term = this.value.trim();
            if (searchTimeout) {
                clearTimeout(searchTimeout);
            }

            if (term.length < 2) {
                searchResults.innerHTML = '<div class=\"header-search-empty\">Arama yapmak için en az 2 karakter yazın.</div>';
                return;
            }

            searchTimeout = setTimeout(function() {
                fetch('/Search/Global?term=' + encodeURIComponent(term))
                    .then(r => r.json())
                    .then(data => {
                        if (!Array.isArray(data) || data.length === 0) {
                            searchResults.innerHTML = '<div class=\"header-search-empty\">Sonuç bulunamadı.</div>';
                            return;
                        }
                        const fragment = document.createDocumentFragment();
                        data.forEach(item => {
                            const btn = document.createElement('button');
                            btn.type = 'button';
                            btn.className = 'header-search-result';
                            btn.dataset.url = item.url;

                            const main = document.createElement('div');
                            main.className = 'header-search-result-main';

                            const name = document.createElement('div');
                            name.className = 'header-search-result-name';
                            name.textContent = item.name;
                            main.appendChild(name);

                            if (item.category) {
                                const meta = document.createElement('div');
                                meta.className = 'header-search-result-meta';
                                meta.textContent = item.category;
                                main.appendChild(meta);
                            }

                            const type = document.createElement('div');
                            type.className = 'header-search-result-type';
                            type.textContent = item.type === 'product' ? 'Ürün' : 'Kategori';

                            btn.appendChild(main);
                            btn.appendChild(type);

                            btn.addEventListener('click', function() {
                                const url = this.dataset.url;
                                if (url) {
                                    window.location.href = url;
                                }
                            });

                            fragment.appendChild(btn);
                        });

                        searchResults.innerHTML = '';
                        searchResults.appendChild(fragment);
                    })
                    .catch(() => {
                        searchResults.innerHTML = '<div class=\"header-search-empty\">Arama sırasında bir hata oluştu.</div>';
                    });
            }, 250);
        });
    }
});
