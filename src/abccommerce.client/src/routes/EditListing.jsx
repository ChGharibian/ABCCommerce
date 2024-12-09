import { useEffect, useState } from 'react';
import { useParams, useNavigate } from 'react-router-dom';
import Input from '../components/Input';
import { CurrencyUtil } from '../util/currency';
import { ArrayUtil } from '../util/arrays';
import TagList from '../components/TagList';
import ImageInput from '../components/ImageInput';
import './EditListing.css';
import { useCookies } from 'react-cookie';
import ImageScrollerDelete from '../components/ImageScrollerDelete';

export default function EditListing({}) {
    const [currentTag, setCurrentTag] = useState('');
    const { sellerId, listingId } = useParams();
    const [cookies] = useCookies(['userToken']);
    const navigate = useNavigate();
    const [tagList, setTagList] = useState([]);
    const [oldImages, setOldImages] = useState([]);
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
                active: data.active,
                removeImages: [],
                newImages: [], 
                imageIds: data.imageIds
            })

            setTagList(data.tags);

            setOldImages(data.images);
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

        // attempt to edit listing
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
                return;
            }
        }
        catch(error) {
            setErrors({...errors, submit: "An error occured submitting your edit request"})
            return;
        }

        let serverErr = false;

        let deleteCountFail = 0;
        if(listingData.removeImages.length > 0) {
            // attempt to remove images
            
            for(const index of listingData.removeImages) {
                try{
                    let res = await fetch(`http://localhost:5147/seller/${sellerId}/listings/${listingId}/image/${listingData.imageIds[index]}`, {
                        method: "DELETE",
                        headers: {
                            "Authorization": "Bearer " + cookies.userToken
                        }
                    });
                    if(!res.ok) deleteCountFail++;
                }
                catch(error) {
                    setErrors({...errors, image: "A server error occured deleting images"})
                    serverErr = true;
                }
                
            }

            if(deleteCountFail > 0) {
                setErrors({...errors, image: `${deleteCountFail} images failed to delete`})
            }
            
        }

        let uploadCountFail = 0;
        if(listingData.newImages.length > 0) {
            // attempt to add images
            for(const image of listingData.newImages) {
                try{
                    let res = await fetch(`http://localhost:5147/seller/${sellerId}/listings/${listingId}/image`, {
                        method: "POST",
                        headers: {
                            "Authorization": "Bearer " + cookies.userToken,
                            "Content-Type": "application/json"
                        },
                        body: JSON.stringify({
                            image: image.encoding,
                            fileType: image.extension
                        })
                    });
                    if(!res.ok) uploadCountFail++;
                }
                catch(error) {
                    setErrors({...errors, image: "A server error occured uploading images"})
                    serverErr = true;
                }
                
            }

            if(uploadCountFail > 0) {
                setErrors({...errors, image: `${uploadCountFail} images failed to upload`})
            }
        }

        // check for upload / delete errors

        let path;
        if(listingData.active) {
            path = `/seller/${sellerId}/listing/${listingId}`
        } else {
            path = `/seller/${sellerId}`
        }
        if(uploadCountFail > 0 || deleteCountFail > 0 || serverErr) {
            setTimeout(() => {
                navigate(path)
            }, 5000);
        } else {
            navigate(path);
        }
    }

    return (
        <div id="add-listing-page-wrapper">
        <h1>Edit Listing</h1>
        <form id="add-listing-wrapper" onKeyDown={e => { if(e.key === 'Enter') e.preventDefault(); }}>
            <button className="hidden" type="submit" onSubmit={e => e.preventDefault()}></button>
            {oldImages.length > 0 &&
                <ImageScrollerDelete images={oldImages} handleChange={(removeList) => { setListingData({...listingData, removeImages: removeList})} }/>
            }
            <div id="add-listing-image-wrapper">
                <ImageInput images={listingData.newImages} handleImageInput={(i) => setListingData({...listingData, newImages: i})}/>
            </div>
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