$(document).ready(function () {
    const products = [
        { name: "Produkt 1", price: "$10", img: "https://via.placeholder.com/150" },
        { name: "Produkt 2", price: "$15", img: "https://via.placeholder.com/150" },
        { name: "Produkt 3", price: "$20", img: "https://via.placeholder.com/150" }
    ];

    $("#show-products").click(function () {
        $("#product-list").empty();
        products.forEach(product => {
            $("#product-list").append(`
                <div class="product">
                    <img src="${product.img}" alt="${product.name}">
                    <h3>${product.name}</h3>
                    <p>${product.price}</p>
                </div>
            `);
        });
    });
});