export function handlePageChange(newPage, currentPage, set, listings) {
    if(listings?.length === 0 && newPage > currentPage) return;
    set(newPage);
}