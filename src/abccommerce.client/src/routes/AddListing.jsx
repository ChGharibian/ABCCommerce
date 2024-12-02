import './AddListing.css';
import Input from '../components/Input';
import { useState } from 'react';
import TagList from '../components/TagList';
import { useNavigate, useParams } from 'react-router-dom';
import { getDollarString } from '../util/currency';
import ImageInput from '../components/ImageInput';
export default function AddListing() {
    const [currentTag, setCurrentTag] = useState('');
    const { sellerId } = useParams();
    const [tagList, setTagList] = useState([]);
    const [newSKU, setNewSKU] = useState("");
    const navigate = useNavigate();
    const [errors, setErrors] = useState({

    })
    const [images, setImages] = useState([]);
    const [listingData, setListingData] = useState({
        name: '',
        description: '',
        quantity: 1,
        price: 0,
        tags: [],
    });
    const [itemData, setItemData] = useState({
        name: '',
        sku: '',
    })
    const [useExistingItem, setUseExistingItem] = useState(false);

    function handleListingChange(e) {
        e.preventDefault();
        let {name, value} = e.target;
        if(name === 'quantity') {
            if(e.target.value < 1) return;
        } 
        if(name === 'price') {
            if(!/\d*\.?\d*/.test(e.target.value)) return;
        }
        setListingData({...listingData, [name]: value})
    }

    function handleItemChange(e) {
        e.preventDefault();
        let {name, value} = e.target;
        setItemData({...itemData, [name]: value})
    }

    function handleTagChange(e) {
        setCurrentTag(e.target.value);
    }

    function handleTagKeyDown(e) {
        if(e.key === 'Enter' && currentTag.length > 0 && !/^\s+$/.test(currentTag)) {
            // add tag to form data 
            let updatedTagList = [...listingData.tags, currentTag.trim()];
            setListingData({...listingData, tags: updatedTagList});
            setTagList(updatedTagList);
            setCurrentTag('');
        }
    }

    function removeTag(e) {
        let index = [...e.target.parentNode.children].indexOf(e.target);
        let updatedTagList = [...tagList.slice(0, index), ...tagList.slice(index + 1, tagList.length)];
        setTagList(updatedTagList);
        setListingData({...listingData, tags: updatedTagList});
    }

    async function generateSKU() {
        let skuIsntUnique = true;
        let sku;
        while(skuIsntUnique) {
            // no leading 0
            let firstDigit = Math.ceil(Math.random() * 10).toString();
            let rest = Math.random().toFixed(12).slice(2);
            sku = firstDigit + rest;
            // check if sku exists
            try{
                let response = await fetch(`http://localhost:5147/seller/${sellerId}/items/${sku}/exists`);
                let data = await response.json();
                skuIsntUnique = data.exists;
            }
            catch(error) {
                console.error(error);
                return "";
            }
            
        }
        return sku;
    }

    async function handleSKUButton(e) {
        e.preventDefault();
        let sku = await generateSKU();
        setNewSKU(sku);
        setItemData({...itemData, sku})
    }

    function handleSKUChange(e) {
        e.preventDefault();
        setNewSKU(e.target.value);
        setItemData({...itemData, sku: e.target.value})
    }

    async function handleSubmit(e) {
        e.preventDefault();
        listingData.price = Number(listingData.price);
        listingData.quantity = Number(listingData.quantity);
        let item = itemData;
        if(useExistingItem) {
            try {
                let response = await fetch(`http://localhost:5147/seller/${sellerId}/items/${itemData.sku}`);
                if(!response.ok) {
                    setErrors({...errors, existingSKU: "SKU not found"})
                    return;
                }
                item = await response.json();
                item.seller = Number(sellerId);
            }
            catch(error) {
                setErrors({...errors, existingSKU: "Error encountered while searching for item"});
                return;
            }
        } else {
            // adding new item first
            try {
                // check to see if item sku exists already
                let response = await fetch(`http://localhost:5147/seller/${sellerId}/items/${itemData.sku}/exists`);
                let data = await response.json();
                if(data.exists) {
                    setErrors({...errors, newSKU: "SKU already exists"});
                    return;
                }
                // attempt to add item
                response = await fetch('http://localhost:5147/item', {
                    method: "POST",
                    headers: {
                        "Content-Type": "application/json"
                    },
                    body: JSON.stringify({...itemData, seller: Number(sellerId)})
                })

                if(!response.ok) {
                    setErrors({...errors, newSKU: "Error occured while adding item"});
                    return;
                }
                item = await response.json();
            }
            catch(error) {
                setErrors({...errors, newSKU: "Error occured while adding item"})
                return;
            }
        }

        // attempt to add listing
        try {
            let response = await fetch('http://localhost:5147/listing', {
                method: "POST",
                headers: {
                    "Content-Type": "application/json"
                },
                body: JSON.stringify({
                    ...listingData,
                    active: true,
                    item: item.id
                })
            })

            if(!response.ok) {
                setErrors({...errors, listing: "Error occured while adding listing"});
                return;
            }

            let listing = await response.json();
            navigate(`/seller/${sellerId}/listing/${listing.id}`);
            
        }
        catch(error) {
            setErrors({...errors, listing: "Error occured while adding listing"})
        }
    }
    
    return (
        <div id="add-listing-page-wrapper">
        <h1>Add Listing</h1>
        <form id="add-listing-wrapper" onKeyDown={e => { if(e.key === 'Enter') e.preventDefault(); }}>
            <button className="hidden" type="submit" onSubmit={e => e.preventDefault()}></button>
            <div id="add-listing-image-wrapper">
                <ImageInput images={images} handleImageInput={(i) => setImages(i)}/>
            </div>
            <Input placeholder="Name" required={true} name="name" value={listingData.name} onChange={handleListingChange}/>
            <textarea placeholder="Description" type="textarea" name="description" value={listingData.description} onChange={handleListingChange}/>
            <Input placeholder="Quantity" type="number" name="quantity" value={listingData.quantity} onChange={handleListingChange} required={true} />
            <Input placeholder="Price" name="price" value={listingData.price} onChange={handleListingChange} required={true} />
            <p>{getDollarString(listingData.price)}</p>
            <Input placeholder="Tags (Press enter to add a tag)" value={currentTag} name="tags" onKeyDown={handleTagKeyDown} onChange={handleTagChange}/>            
            <div className={tagList.length === 0 ? 'hidden' : ''}>  
                <TagList tags={tagList} onClick={removeTag}/>
            </div>
            <label htmlFor="item-check">Create listing with existing item?<input type="checkbox" name="item-check" value={useExistingItem} onChange={e => setUseExistingItem(e.target.checked)}/></label>
            {useExistingItem ?
                <Input placeholder="Enter existing item SKU" error={errors.existingSKU} name="sku" onChange={handleItemChange} required={true} />
                
            : 
                <div id="add-listing-new-item-wrapper">
                    <Input value={itemData.name} onChange={handleItemChange} name="name" placeholder="Item name" required={true}/>
                    <div className="input-with-button">
                        <Input value={newSKU} error={errors.newSKU} onChange={handleSKUChange} placeholder="Enter new item SKU" required={true} />
                        <button onClick={handleSKUButton}>Generate</button>
                    </div>
                </div>
            }
            <button id="submit-add-listing" onClick={handleSubmit}>Add</button>
            
        </form>
        </div>
    )
}