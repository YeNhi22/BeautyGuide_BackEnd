// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Beauty Guide Website JavaScript

// Document Ready
document.addEventListener('DOMContentLoaded', function() {
    // Scroll reveal animation
    const revealElements = document.querySelectorAll('.reveal');
    
    function checkReveal() {
        const windowHeight = window.innerHeight;
        const revealPoint = 150;
        
        revealElements.forEach(element => {
            const revealTop = element.getBoundingClientRect().top;
            
            if (revealTop < windowHeight - revealPoint) {
                element.classList.add('active');
            }
        });
    }
    
    // Initial check
    checkReveal();
    
    // Check on scroll
    window.addEventListener('scroll', checkReveal);
    
    // Navbar scroll effect
    const navbar = document.querySelector('.navbar');
    
    window.addEventListener('scroll', function() {
        if (window.scrollY > 50) {
            navbar.classList.add('shadow');
            navbar.style.padding = '10px 0';
        } else {
            navbar.classList.remove('shadow');
            navbar.style.padding = '15px 0';
        }
    });
    
    // Smooth scroll for anchor links
    document.querySelectorAll('a[href^="#"]').forEach(anchor => {
        anchor.addEventListener('click', function(e) {
            e.preventDefault();
            
            const targetId = this.getAttribute('href');
            if (targetId === '#') return;
            
            const targetElement = document.querySelector(targetId);
            if (targetElement) {
                window.scrollTo({
                    top: targetElement.offsetTop - 100,
                    behavior: 'smooth'
                });
            }
        });
    });
    
    // Counter animation
    const counterItems = document.querySelectorAll('.counter-number');
    
    function animateCounter() {
        counterItems.forEach(counter => {
            const target = parseInt(counter.innerText.replace(/[^\d]/g, ''));
            const suffix = counter.innerText.replace(/[\d]/g, '');
            let count = 0;
            const duration = 2000; // 2 seconds
            const interval = Math.floor(duration / target);
            
            const counterAnimation = setInterval(() => {
                count += 1;
                counter.innerText = count + suffix;
                
                if (count >= target) {
                    counter.innerText = target + suffix;
                    clearInterval(counterAnimation);
                }
            }, interval);
        });
    }
    
    // Trigger counter animation when in viewport
    const counterSection = document.querySelector('.counter-item');
    if (counterSection) {
        const observer = new IntersectionObserver((entries) => {
            entries.forEach(entry => {
                if (entry.isIntersecting) {
                    animateCounter();
                    observer.unobserve(entry.target);
                }
            });
        }, { threshold: 0.5 });
        
        observer.observe(counterSection);
    }
    
    // Parallax effect for hero section
    const heroSection = document.querySelector('.hero-section');
    if (heroSection) {
        window.addEventListener('scroll', () => {
            const scrollPosition = window.scrollY;
            if (scrollPosition < 600) {
                heroSection.style.backgroundPositionY = `${scrollPosition * 0.5}px`;
            }
        });
    }
    
    // Add hover effects for cards with JavaScript
    const cards = document.querySelectorAll('.blog-card, .category-card, .featured-card, .testimonial-card');
    
    cards.forEach(card => {
        card.addEventListener('mouseenter', function() {
            this.style.transform = 'translateY(-10px)';
            this.style.boxShadow = '0 15px 30px rgba(0, 0, 0, 0.1)';
        });
        
        card.addEventListener('mouseleave', function() {
            this.style.transform = 'translateY(0)';
            this.style.boxShadow = '0 5px 15px rgba(0, 0, 0, 0.07)';
        });
    });
    
    // Add tilt effect to hero card
    const heroCard = document.querySelector('.hero-card');
    if (heroCard) {
        heroCard.addEventListener('mousemove', function(e) {
            const cardRect = this.getBoundingClientRect();
            const cardCenterX = cardRect.left + cardRect.width / 2;
            const cardCenterY = cardRect.top + cardRect.height / 2;
            const mouseX = e.clientX;
            const mouseY = e.clientY;
            
            // Calculate tilt values (max 15 degrees)
            const tiltX = ((mouseY - cardCenterY) / (cardRect.height / 2)) * 5;
            const tiltY = ((mouseX - cardCenterX) / (cardRect.width / 2)) * -5;
            
            // Apply tilt
            this.style.transform = `perspective(1000px) rotateX(${tiltX}deg) rotateY(${tiltY}deg)`;
        });
        
        heroCard.addEventListener('mouseleave', function() {
            this.style.transform = 'perspective(1000px) rotateX(0deg) rotateY(0deg)';
        });
    }

    // Blog post animation
    const blogCards = document.querySelectorAll('.blog-card');
    if (blogCards.length > 0) {
        blogCards.forEach((card, index) => {
            card.style.animationDelay = `${index * 0.1}s`;
        });
    }

    // Blog detail page - Reading progress bar
    const blogContent = document.querySelector('.blog-content');
    if (blogContent) {
        const progressBar = document.createElement('div');
        progressBar.className = 'reading-progress';
        document.body.appendChild(progressBar);

        window.addEventListener('scroll', () => {
            const totalHeight = blogContent.offsetHeight;
            const windowHeight = window.innerHeight;
            const scrolled = window.scrollY - blogContent.offsetTop;
            
            const scrollPercent = (scrolled / (totalHeight - windowHeight)) * 100;
            progressBar.style.width = `${Math.min(100, Math.max(0, scrollPercent))}%`;
        });
    }

    // Image lightbox for blog content
    const blogImages = document.querySelectorAll('.blog-text img');
    if (blogImages.length > 0) {
        blogImages.forEach(img => {
            img.style.cursor = 'pointer';
            img.addEventListener('click', function() {
                const lightbox = document.createElement('div');
                lightbox.className = 'lightbox';
                lightbox.innerHTML = `
                    <div class="lightbox-content">
                        <img src="${this.src}" alt="${this.alt}">
                        <span class="lightbox-close">&times;</span>
                    </div>
                `;
                document.body.appendChild(lightbox);
                
                // Prevent scrolling when lightbox is open
                document.body.style.overflow = 'hidden';
                
                // Close lightbox on click
                lightbox.addEventListener('click', function() {
                    this.remove();
                    document.body.style.overflow = '';
                });
            });
        });
    }

    // Highlight current category in sidebar
    const categoryLinks = document.querySelectorAll('.category-sidebar .list-group-item');
    if (categoryLinks.length > 0) {
        const currentUrl = window.location.href;
        categoryLinks.forEach(link => {
            if (link.href === currentUrl) {
                link.classList.add('active');
            }
        });
    }

    // Animated scroll to top button
    const scrollTopBtn = document.createElement('button');
    scrollTopBtn.className = 'scroll-top-btn';
    scrollTopBtn.innerHTML = '<i class="fas fa-arrow-up"></i>';
    document.body.appendChild(scrollTopBtn);

    window.addEventListener('scroll', () => {
        if (window.scrollY > 300) {
            scrollTopBtn.classList.add('show');
        } else {
            scrollTopBtn.classList.remove('show');
        }
    });

    scrollTopBtn.addEventListener('click', () => {
        window.scrollTo({
            top: 0,
            behavior: 'smooth'
        });
    });

    // Estimated reading time for blog posts
    const blogText = document.querySelector('.blog-text');
    if (blogText) {
        const text = blogText.textContent;
        const wordCount = text.split(/\s+/).length;
        const readingTime = Math.ceil(wordCount / 200); // Average reading speed: 200 words per minute
        
        const readingTimeEl = document.createElement('div');
        readingTimeEl.className = 'reading-time';
        readingTimeEl.innerHTML = `<i class="far fa-clock me-1"></i> ${readingTime} phút đọc`;
        
        const blogHeader = document.querySelector('.blog-detail-hero .post-meta');
        if (blogHeader) {
            blogHeader.appendChild(readingTimeEl);
        }
    }

    // Animate comment form on focus
    const commentForm = document.querySelector('.comment-form textarea');
    if (commentForm) {
        commentForm.addEventListener('focus', function() {
            this.parentElement.style.transform = 'translateY(-5px)';
            this.style.boxShadow = '0 10px 20px rgba(0, 0, 0, 0.1)';
        });
        
        commentForm.addEventListener('blur', function() {
            this.parentElement.style.transform = 'translateY(0)';
            this.style.boxShadow = '';
        });
    }
});

// Newsletter form validation
const newsletterForm = document.querySelector('.newsletter-form');
if (newsletterForm) {
    newsletterForm.addEventListener('submit', function(e) {
        e.preventDefault();
        const emailInput = this.querySelector('input[type="email"]');
        if (emailInput.value.trim() === '') {
            showToast('Vui lòng nhập địa chỉ email của bạn.', 'warning');
        } else if (!isValidEmail(emailInput.value)) {
            showToast('Vui lòng nhập địa chỉ email hợp lệ.', 'warning');
        } else {
            showToast('Cảm ơn bạn đã đăng ký nhận tin!', 'success');
            emailInput.value = '';
            
            // Add success animation
            const button = this.querySelector('button[type="submit"]');
            button.innerHTML = '<i class="fas fa-check"></i> Đã đăng ký';
            button.classList.add('btn-success');
            
            setTimeout(() => {
                button.innerHTML = '<span class="me-2"><i class="fas fa-paper-plane"></i></span>Đăng ký';
                button.classList.remove('btn-success');
            }, 3000);
        }
    });
}

// Toast notification function
function showToast(message, type = 'info') {
    const toastContainer = document.querySelector('.toast-container');
    const toast = document.createElement('div');
    toast.className = `toast toast-${type}`;
    toast.innerHTML = `
        <div class="toast-content">
            <i class="fas ${type === 'success' ? 'fa-check-circle' : type === 'warning' ? 'fa-exclamation-circle' : 'fa-info-circle'}"></i>
            <div class="message">${message}</div>
        </div>
        <div class="toast-progress"></div>
    `;
    
    toastContainer.appendChild(toast);
    
    setTimeout(() => {
        toast.classList.add('show');
        
        setTimeout(() => {
            toast.classList.remove('show');
            setTimeout(() => {
                toast.remove();
            }, 300);
        }, 3000);
    }, 10);
}

// Email validation helper
function isValidEmail(email) {
    const re = /^(([^<>()\[\]\\.,;:\s@"]+(\.[^<>()\[\]\\.,;:\s@"]+)*)|(".+"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/;
    return re.test(String(email).toLowerCase());
}

// Add active class to current nav item
document.addEventListener("DOMContentLoaded", function() {
    const currentPath = window.location.pathname.toLowerCase();
    const navLinks = document.querySelectorAll('.navbar-nav .nav-link');
    
    navLinks.forEach(link => {
        const href = link.getAttribute('href').toLowerCase();
        if (currentPath === href || (href !== '/' && currentPath.startsWith(href))) {
            link.classList.add('active');
        }
    });
});

// Copy code blocks in blog posts
document.addEventListener('DOMContentLoaded', function() {
    const codeBlocks = document.querySelectorAll('.blog-text pre');
    if (codeBlocks.length > 0) {
        codeBlocks.forEach(block => {
            const copyBtn = document.createElement('button');
            copyBtn.className = 'copy-code-btn';
            copyBtn.innerHTML = '<i class="far fa-copy"></i>';
            copyBtn.title = 'Sao chép';
            
            block.style.position = 'relative';
            block.appendChild(copyBtn);
            
            copyBtn.addEventListener('click', function() {
                const code = block.querySelector('code') ? block.querySelector('code').innerText : block.innerText;
                navigator.clipboard.writeText(code).then(() => {
                    copyBtn.innerHTML = '<i class="fas fa-check"></i>';
                    setTimeout(() => {
                        copyBtn.innerHTML = '<i class="far fa-copy"></i>';
                    }, 2000);
                });
            });
        });
    }
});
