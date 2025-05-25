/*=============== SHOW MENU ===============*/
const navMenu = document.getElementById("nav-menu"),
  navToggle = document.getElementById("nav-toggle"),
  navClose = document.getElementById("nav-close");

/*===== Menu Show =====*/
/* Validate if constant exists */
if (navToggle) {
  navToggle.addEventListener("click", () => {
    navMenu.classList.add("show-menu");
  });
}

/*===== Hide Show =====*/
/* Validate if constant exists */
if (navClose) {
  navClose.addEventListener("click", () => {
    navMenu.classList.remove("show-menu");
  });
}

/*=============== IMAGE GALLERY ===============*/
function imgGallery() {
  const mainImg = document.querySelector(".details__img"),
    smallImg = document.querySelectorAll(".details__small-img");

  smallImg.forEach((img) => {
      img.addEventListener("click", function () {
          const temp = mainImg.src;
          mainImg.src = this.src;
          this.src = temp;  
    });

  });
}

imgGallery();

/*=============== SWIPER CATEGORIES ===============*/
let swiperCategories = new Swiper(".categories__container", {
  spaceBetween: 24,
  loop: true,
  navigation: {
    nextEl: ".swiper-button-next",
    prevEl: ".swiper-button-prev",
  },

  breakpoints: {
    350: {
      slidesPerView: 2,
      spaceBetween: 24,
    },
    768: {
      slidesPerView: 3,
      spaceBetween: 24,
    },
    992: {
      slidesPerView: 4,
      spaceBetween: 24,
    },
    1200: {
      slidesPerView: 5,
      spaceBetween: 24,
    },
    1400: {
      slidesPerView: 6,
      spaceBetween: 24,
    },
  },
});

/*=============== SWIPER PRODUCTS ===============*/
let swiperProducts = new Swiper(".new__container", {
  spaceBetween: 24,
  loop: true,
  navigation: {
    nextEl: ".swiper-button-next",
    prevEl: ".swiper-button-prev",
  },

  breakpoints: {
    768: {
      slidesPerView: 2,
      spaceBetween: 24,
    },
    992: {
      slidesPerView: 4,
      spaceBetween: 24,
    },
    1400: {
      slidesPerView: 4,
      spaceBetween: 24,
    },
  },
});

/*=============== PRODUCTS TABS ===============*/
const tabs = document.querySelectorAll("[data-target]"),
  tabsContents = document.querySelectorAll("[content]");

tabs.forEach((tab) => {
  tab.addEventListener("click", () => {
    const target = document.querySelector(tab.dataset.target);

    tabsContents.forEach((tabsContent) => {
      tabsContent.classList.remove("active-tab");
    });

    target.classList.add("active-tab");

    tabs.forEach((tab) => {
      tab.classList.remove("active-tab");
    });

    tab.classList.add("active-tab");    
  });
});

///---------------Categories Meun-----------------//
function toggleCategories() {
    const categoryMenu = document.getElementById('category-menu');
    if (categoryMenu.style.display === "none") {
        categoryMenu.style.display = "block";  // Show the menu
    } else {
        categoryMenu.style.display = "none";  // Hide the menu
    }
}

function toggleSubcategories(btn, categoryName) {
    console.log(btn);   
    const subcategoryList = document.getElementById(categoryName);

    if (subcategoryList) {
        subcategoryList.classList.toggle('hidden');
        btn.textContent = btn.textContent == '-' ? '+' : '-';
//        btn.textContent.style = "bold";


    }
}
///----Pagnation-------//



// this code generate two pages beore the current page and two pages ater the current page
// also generate << >>  works if the current page is not the first or last page in the pagination
// also  generate the first and the last page if the current page is not the first or last page in the pagination

// Function to generate pagination links dynamically
function generatePagination() {
    const pagination = document.getElementById('pagination');
    if (!pagination) return;

    pagination.innerHTML = '';
    const categoryId = pagination.getAttribute('CategoryId');
    const subCategoryId = pagination.getAttribute('SubCategoryId');
    const priceFilter = pagination.getAttribute('PriceFilter'); // Fixed variable name (lowercase)
    const brandFilter = pagination.getAttribute('BrandFilter'); // This will be a comma-separated string
    const tagFilter = pagination.getAttribute('TagFilter');
    const name = pagination.getAttribute('Name');
    const totalPages = parseInt(pagination.getAttribute('TotalPages'), 10);
    const currentPage = parseInt(pagination.getAttribute('currentPage'), 10);

    // Page range calculation
    const pageRange = 2;
    let startPage = Math.max(1, currentPage - pageRange);
    let endPage = Math.min(totalPages, currentPage + pageRange);

    // Helper function to build URL with all parameters
    const buildUrl = (pageNum) => {
        const params = new URLSearchParams();
        params.append('pageNumber', pageNum);
        if (categoryId) params.append('CategoryId', categoryId);
        if (subCategoryId) params.append('SubCategoryId', subCategoryId);
        if (priceFilter) params.append('PriceFilter', priceFilter);
        if (brandFilter) {
            // Split the comma-separated brands and add each one
            brandFilter.split(',').forEach(brand => {
                params.append('BrandFilter', brand.trim());
            });
        }
        if (tagFilter) params.append('TagFilter', tagFilter);
        if (name) params.append('Name', name);
        return `?${params.toString()}`;
    };

    // Previous button
    const prevLi = document.createElement('li');
    const prevLink = document.createElement('a');
    prevLink.href = currentPage > 1 ? buildUrl(currentPage - 1) : '#';
    prevLink.className = `pagination__link icon ${currentPage === 1 ? 'disabled' : ''}`;
    prevLink.innerHTML = '<i class="fi-rs-angle-double-small-right"></i>';
    prevLi.appendChild(prevLink);
    pagination.appendChild(prevLi);

    // First page and ellipsis
    if (startPage > 1) {
        const firstLi = document.createElement('li');
        const firstLink = document.createElement('a');
        firstLink.href = buildUrl(1);
        firstLink.className = 'pagination__link';
        firstLink.textContent = '1';
        firstLi.appendChild(firstLink);
        pagination.appendChild(firstLi);

        if (startPage > 2) {
            const ellipsisLi = document.createElement('li');
            ellipsisLi.innerHTML = '<span class="pagination__link">...</span>';
            pagination.appendChild(ellipsisLi);
        }
    }

    // Page links
    for (let i = startPage; i <= endPage; i++) {
        const pageLi = document.createElement('li');
        const pageLink = document.createElement('a');
        pageLink.href = buildUrl(i);
        pageLink.className = `pagination__link ${i === currentPage ? 'active' : ''}`;
        pageLink.textContent = i;
        pageLi.appendChild(pageLink);
        pagination.appendChild(pageLi);
    }

    // Last page and ellipsis
    if (endPage < totalPages) {
        if (endPage < totalPages - 1) {
            const ellipsisLi = document.createElement('li');
            ellipsisLi.innerHTML = '<span class="pagination__link">...</span>';
            pagination.appendChild(ellipsisLi);
        }

        const lastLi = document.createElement('li');
        const lastLink = document.createElement('a');
        lastLink.href = buildUrl(totalPages);
        lastLink.className = 'pagination__link';
        lastLink.textContent = totalPages;
        lastLi.appendChild(lastLink);
        pagination.appendChild(lastLi);
    }

    // Next button
    const nextLi = document.createElement('li');
    const nextLink = document.createElement('a');
    nextLink.href = currentPage < totalPages ? buildUrl(currentPage + 1) : '#';
    nextLink.className = `pagination__link icon ${currentPage === totalPages ? 'disabled' : ''}`;
    nextLink.innerHTML = '<i class="fi-rs-angle-double-small-right"></i>';
    nextLi.appendChild(nextLink);
    pagination.appendChild(nextLi);
}
// Initial pagination render
generatePagination();

//-----------------------------------------------------------//

function pricefilter() {
    const priceInput = document.getElementById('PriceFilter');
    const userInput = document.getElementById('userInput');
    priceInput.addEventListener('input', () => {
        const value = priceInput.value;
        userInput.textContent = `$${value}`;
    });


}
pricefilter();
