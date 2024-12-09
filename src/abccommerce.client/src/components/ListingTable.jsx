import './ListingTable.css'
const ListingTable = ({ cartItems, totalPrice, CurrencyUtil }) => {
  return (
    <div id="listing-table-wrapper">
      <table>
        <thead>
          <tr>
            <th scope="col">Name</th>
            <th scope="col">Description</th>
            <th scope="col">Quantity</th>
            <th scope="col">Cost</th>
          </tr>
        </thead>
        <tbody>
          {cartItems.map((currentListing) => (
            <tr key={currentListing.id}>
              <td data-label="Name">{currentListing.listing.name}</td>
              <td data-label="Description">{currentListing.listing.description}</td>
              <td data-label="Quantity">{currentListing.quantity}</td>
              <td data-label="Cost">
                {CurrencyUtil.getDollarString(currentListing.listing.pricePerUnit * currentListing.quantity)}
              </td>
            </tr>
          ))}
        </tbody>
        <tfoot>
          <tr>
            <th scope="row" colSpan="3">Total Cost</th>
            <td>{CurrencyUtil.getDollarString(totalPrice)}</td>
          </tr>
        </tfoot>
      </table>
    </div>
  );
};

export default ListingTable;