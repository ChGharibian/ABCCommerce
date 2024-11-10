import Arrow from './Arrow';
import './PageSelector.css';
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