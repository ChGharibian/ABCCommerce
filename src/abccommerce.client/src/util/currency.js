export function getDollarString(price) {
    return '$' + Number.parseFloat(price).toFixed(2);
}