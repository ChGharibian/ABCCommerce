import { useEffect, useState } from 'react';
import { useParams, useNavigate } from 'react-router-dom';
import Input from '../components/Input';
import { CurrencyUtil } from '../util/currency';
import { ArrayUtil } from '../util/arrays';
import TagList from '../components/TagList';
import './EditListing.css';
import { useCookies } from 'react-cookie';

export default function EditListing({}) {
    const [currentTag, setCurrentTag] = useState('');
    const { sellerId, listingId } = useParams();
    const [cookies] = useCookies(['userToken']);
    const navigate = useNavigate();
    const [tagList, setTagList] = useState([]);
    const [errors, setErrors] = useState({});
    const [listingData, setListingData] = useState({
        description: '',
        quantity: 1,
        price: 0,
        originalTags: [],
        tags: [],
        active: true,
    });

    useEffect(() => {
        getListingData();
    },[])

    async function getListingData() {
        try {
            let response = await fetch(`http://localhost:5147/listing/${listingId}`);
            if(!response.ok) {
                setErrors({...errors, server: "Something went wrong fetching the listing"});
                return;
            }
            let data = await response.json();
            setListingData({
                description: data.description, 
                quantity: data.quantity,
                price: data.pricePerUnit,
                originalTags: data.tags,
                active: data.active
            })

            setTagList(data.tags);
        }
        catch(error) {
            setErrors({...errors, server: "Something went wrong fetching the listing"});
        }
    }

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

    function handleTagChange(e) {
        setCurrentTag(e.target.value);
    }

    function handleTagKeyDown(e) {
        if(e.key === 'Enter' && currentTag.length > 0 && !/^\s+$/.test(currentTag)) {
            // add tag to form data 
            let updatedTagList = [...tagList, currentTag.trim()];
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

    async function handleSubmit(e) {
        e.preventDefault();
        let tagDiff = ArrayUtil.getAddedRemovedItems(listingData.originalTags, tagList);
        try {
            let response = await fetch(`http://localhost:5147/seller/${sellerId}/listings/${listingId}`, {
                method: "PATCH",
                headers: {
                    "Content-Type": "application/json",
                    "Authorization": "Bearer " + cookies.userToken
                },
                body: JSON.stringify({
                    quantity: listingData.quantity,
                    description: listingData.description,
                    active: listingData.active,
                    price: listingData.price,
                    addTags: tagDiff.added,
                    removeTags: tagDiff.removed
                })
            })

            if(!response.ok) {
                setErrors({...errors, submit: "An error occured submitting your edit request"})
            } else {
                if(listingData.active) {
                    navigate(`/seller/${sellerId}/listing/${listingId}`);
                } else {
                    navigate(`/seller/${sellerId}`);
                }

            }
        }
        catch(error) {
            setErrors({...errors, submit: "An error occured submitting your edit request"})
        }
    }

    return (
        <div id="add-listing-page-wrapper">
        <h1>Edit Listing</h1>
        <form id="add-listing-wrapper" onKeyDown={e => { if(e.key === 'Enter') e.preventDefault(); }}>
            <button className="hidden" type="submit" onSubmit={e => e.preventDefault()}></button>
            <textarea placeholder="Description" type="textarea" name="description" value={listingData.description} onChange={handleListingChange}/>
            <Input placeholder="Quantity" type="number" name="quantity" value={listingData.quantity} onChange={handleListingChange} required={true} />
            <Input placeholder="Price" name="price" value={listingData.price} onChange={handleListingChange} required={true} />
            <p>{CurrencyUtil.getDollarString(listingData.price)}</p>
            <Input placeholder="Tags (Press enter to add a tag)" value={currentTag} name="tags" onKeyDown={handleTagKeyDown} onChange={handleTagChange}/>            
            <div className={tagList.length === 0 ? 'hidden' : ''}>  
                <TagList tags={tagList} onClick={removeTag}/>
            </div>
            <label htmlFor="active-check">Listing active?<input type="checkbox" checked={listingData.active} name="active-check" value={listingData.active} onChange={e => setListingData({...listingData, active: e.target.checked})}/></label>
            <button id="submit-add-listing" onClick={handleSubmit}>Edit</button>
            <p className="error">{errors.submit}</p>
        </form>
        </div>
    )
}