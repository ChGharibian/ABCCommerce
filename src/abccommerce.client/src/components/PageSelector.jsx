import Arrow from './Arrow';
import './PageSelector.css';
/**
 * @category component
 * @function PageSelector
 * @author Thomas Scott
 * @description Displays a tool at the bottom of the page used to 
 * switch between pages, providing an abstract functionality for paging.
 * @since October 24
 * @param {Object} props
 * @param {Number} props.width Width of the page selector
 * @param {Number} props.height Height of the page selector
 * @param {Function} props.handlePageChange Passed in callback function that 
 * is called when a page is changed, passing in the new page number 
 * as a parameter. 
 * @param {Number} props.page Page number passed in to provide synchronization between
 * this component and the component using it.
 * @returns {JSX.Element}
 */
export default function PageSelector({width, height, handlePageChange, page}) {
    // width and height passed in as integers representing pixels

    const downPage = () => {
        if(page > 1) handlePageChange(page - 1)
    }

    const upPage = () => {
        handlePageChange(page + 1)
    }

    return (
        <div className="page-selector-wrapper" style={{
            width,
            height,
            top: "calc(100% - 1rem - 1px - " + height + "px)",
            left: "calc(50% - " + (width / 2) + "px)" 
        }}>
            <Arrow direction="left" size={(height / 4)} onClick={downPage} margin="0 1rem" dimAmount="70%" thickness={2}/>
            <div className="page-number" style={{
                fontSize: (height / 2) + "px"
            }}>{page}</div>
            <Arrow direction="right" size={(height / 4)} onClick={upPage} margin="0 1rem" dimAmount="70%" thickness={2}/>
        </div>
    )
}   