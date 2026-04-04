/* ════════════════════════════════════════════════════════
   Valhalla Bebidas — Lenis, âncoras e animações por página
════════════════════════════════════════════════════════ */

gsap.registerPlugin(ScrollTrigger);

function getPage() {
    const path = window.location.pathname.toLowerCase();

    if (path === '/' || path === '/home' || path === '/home/index') return 'home';
    if (path.includes('/auth/login')) return 'login';
    if (path.includes('/auth/cadastro')) return 'cadastro';
    if (path.includes('/catalogo/produto')) return 'produto';
    if (path.includes('/catalogo')) return 'catalogo';
    if (path.includes('/carrinho/checkout')) return 'checkout';
    if (path.includes('/carrinho')) return 'carrinho';

    return 'other';
}

const prefersReducedMotion = window.matchMedia('(prefers-reduced-motion: reduce)').matches;
const page = getPage();

/* ─── LENIS — recommended initialization (use static include in _Layout) ─── */
if (typeof Lenis === 'undefined') {
    console.warn('Lenis não carregado — usando comportamento de scroll nativo mais suave como fallback. \nConsidere incluir lenis via <script> no _Layout para melhor experiência.');
    try { document.documentElement.style.scrollBehavior = 'smooth'; } catch {}
} else {
    // Recommended parameters for a responsive, natural feeling scroll
    const lenis = new Lenis({
        duration: 0.45, // slightly faster than before for responsiveness
        easing: (t) => Math.min(1, 1.001 - Math.pow(2, -10 * t)),
        smooth: true,
        smoothTouch: true,
        syncTouch: false,
    });

    lenis.on('scroll', ScrollTrigger.update);

    // Integrate with GSAP ticker
    gsap.ticker.add((time) => {
        if (lenis && typeof lenis.raf === 'function') lenis.raf(time * 1000);
    });
    gsap.ticker.lagSmoothing(0);

    // Smooth anchor links using lenis
    document.querySelectorAll('a[href^="#"]').forEach((link) => {
        link.addEventListener('click', (e) => {
            const href = link.getAttribute('href');
            if (!href || href === '#' || href.length < 2) return;
            let target;
            try { target = document.querySelector(href); } catch { return; }
            if (!target) return;
            e.preventDefault();
            lenis.scrollTo(target, {
                offset: -80,
                duration: 0.9,
                easing: (t) => Math.min(1, 1.001 - Math.pow(2, -10 * t)),
            });
        });
    });
}

function initHomeAnimations() {
    if (page !== 'home') return;
    if (prefersReducedMotion) {
        gsap.set(['.stats__item', '.category__card', '.reveal'], { opacity: 1, y: 0 });
        return;
    }

    const statsList = document.querySelector('.stats__list');
    const statsItems = gsap.utils.toArray('.stats__item');
    if (statsList && statsItems.length) {
        gsap.fromTo(
            statsItems,
            { opacity: 0, y: 40 },
            {
                opacity: 1,
                y: 0,
                duration: 1,
                ease: 'power2.out',
                stagger: 0.2,
                scrollTrigger: {
                    trigger: statsList,
                    start: 'top 85%',
                    toggleActions: 'play none none none',
                },
            }
        );
    }

    const categoryList = document.querySelector('.category__second');
    const categoryCards = gsap.utils.toArray('.category__card');
    if (categoryList && categoryCards.length) {
        gsap.fromTo(
            categoryCards,
            { opacity: 0, y: 60 },
            {
                opacity: 1,
                y: 0,
                duration: 0.8,
                ease: 'power2.out',
                stagger: 0.2,
                scrollTrigger: {
                    trigger: categoryList,
                    start: 'top 80%',
                    toggleActions: 'play none none none',
                },
            }
        );
    }

    const revealEls = document.querySelectorAll('.reveal');
    if (revealEls.length) {
        ScrollTrigger.batch('.reveal', {
            start: 'top 85%',
            onEnter: (elements) => {
                gsap.fromTo(
                    elements,
                    { opacity: 0, y: 50 },
                    {
                        opacity: 1,
                        y: 0,
                        duration: 1,
                        ease: 'power2.out',
                        stagger: 0.15,
                        overwrite: true,
                    }
                );
            },
        });
    }
}

function initLoginAnimations() {
    if (page !== 'login' || prefersReducedMotion) return;
    const tl = gsap.timeline({ defaults: { ease: 'power3.out' } });
    tl.fromTo(
        '.login__nav',
        { opacity: 0, y: -14 },
        { opacity: 1, y: 0, duration: 0.55 }
    )
        .fromTo(
            '.login__title_mobile',
            { opacity: 0, y: 22 },
            { opacity: 1, y: 0, duration: 0.62 },
            '-=0.32'
        )
        .fromTo(
            '.login__brand',
            { opacity: 0, x: -36, y: 22 },
            { opacity: 1, x: 0, y: 0, duration: 0.78 },
            '-=0.45'
        )
        .fromTo(
            '.login__form-wrapper',
            { opacity: 0, x: 36, y: 22 },
            { opacity: 1, x: 0, y: 0, duration: 0.78 },
            '-=0.68'
        );
}

function initCadastroAnimations() {
    if (page !== 'cadastro' || prefersReducedMotion) return;
    gsap.fromTo(
        '.cadastro__nav',
        { opacity: 0, y: -14 },
        { opacity: 1, y: 0, duration: 0.55, ease: 'power3.out' }
    );
    gsap.fromTo(
        '.cadastro__header',
        { opacity: 0, y: 22 },
        { opacity: 1, y: 0, duration: 0.68, ease: 'power3.out', delay: 0.08 }
    );
    gsap.utils.toArray('.cadastro__section').forEach((el) => {
        gsap.fromTo(
            el,
            { opacity: 0, y: 22 },
            {
                opacity: 1,
                y: 0,
                duration: 0.62,
                ease: 'power2.out',
                scrollTrigger: {
                    trigger: el,
                    start: 'top 88%',
                    toggleActions: 'play none none none',
                },
            }
        );
    });
}

function initCatalogoAnimations() {
    if (page !== 'catalogo' || prefersReducedMotion) return;
    gsap.fromTo(
        '.catalogoHero .imgSection',
        { opacity: 0, scale: 1.04, y: 20 },
        { opacity: 1, scale: 1, y: 0, duration: 1.05, ease: 'power2.out' }
    );
    gsap.fromTo(
        '.filter-container',
        { opacity: 0, y: 22 },
        {
            opacity: 1,
            y: 0,
            duration: 0.68,
            ease: 'power2.out',
            scrollTrigger: {
                trigger: '.filter-container',
                start: 'top 86%',
                toggleActions: 'play none none none',
            },
        }
    );
    gsap.fromTo(
        '.product__title',
        { opacity: 0, y: 22 },
        {
            opacity: 1,
            y: 0,
            duration: 0.62,
            ease: 'power2.out',
            scrollTrigger: {
                trigger: '.product__title',
                start: 'top 88%',
                toggleActions: 'play none none none',
            },
        }
    );
    if (document.querySelectorAll('.product-card').length) {
        ScrollTrigger.batch('.product-card', {
            start: 'top 91%',
            onEnter: (batch) => {
                gsap.fromTo(
                    batch,
                    { opacity: 0, y: 42 },
                    {
                        opacity: 1,
                        y: 0,
                        duration: 0.52,
                        stagger: 0.055,
                        ease: 'power2.out',
                        overwrite: true,
                    }
                );
            },
        });
    }
}

function initProdutoAnimations() {
    if (page !== 'produto' || prefersReducedMotion) return;
    gsap.fromTo(
        '.produto__breadcrumb',
        { opacity: 0, x: -18, y: 22 },
        { opacity: 1, x: 0, y: 0, duration: 0.48, ease: 'power2.out' }
    );
    const layout = document.querySelector('.produto__layout');
    if (layout) {
        gsap
            .timeline({
                scrollTrigger: {
                    trigger: layout,
                    start: 'top 82%',
                    toggleActions: 'play none none none',
                },
            })
            .fromTo(
                '.produto__img-wrapper',
                { opacity: 0, y: 28 },
                { opacity: 1, y: 0, duration: 0.72, ease: 'power2.out' }
            )
            .fromTo(
                '.produto__info',
                { opacity: 0, y: 28 },
                { opacity: 1, y: 0, duration: 0.72, ease: 'power2.out' },
                '-=0.55'
            );
    }
    gsap.fromTo(
        '.produto__detalhes',
        { opacity: 0, y: 24 },
        {
            opacity: 1,
            y: 0,
            duration: 0.62,
            ease: 'power2.out',
            scrollTrigger: {
                trigger: '.produto__detalhes',
                start: 'top 88%',
                toggleActions: 'play none none none',
            },
        }
    );
}

function initCarrinhoAnimations() {
    if (page !== 'carrinho' || prefersReducedMotion) return;
    const tl = gsap.timeline({ defaults: { ease: 'power3.out' } });
    tl.fromTo(
        '.carrinho__breadcrumb',
        { opacity: 0, x: -14, y: 22 },
        { opacity: 1, x: 0, y: 0, duration: 0.45 }
    )
        .fromTo(
            '.carrinho__header',
            { opacity: 0, y: 22 },
            { opacity: 1, y: 0, duration: 0.52 },
            '-=0.22'
        )
        .fromTo(
            '.carrinho__itens',
            { opacity: 0, y: 22 },
            { opacity: 1, y: 0, duration: 0.58 },
            '-=0.28'
        )
        .fromTo(
            '.carrinho__resumo',
            { opacity: 0, x: 22, y: 22 },
            { opacity: 1, x: 0, y: 0, duration: 0.58 },
            '-=0.42'
        );
}

initHomeAnimations();
initLoginAnimations();
initCadastroAnimations();
initCatalogoAnimations();
initProdutoAnimations();
initCarrinhoAnimations();

window.addEventListener('load', () => {
    ScrollTrigger.refresh();
});
