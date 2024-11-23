import './AddListing.css';
import Input from '../components/Input';
import { useState } from 'react';
import TagList from '../components/TagList';
import { useParams } from 'react-router-dom';
export default function AddListing() {
    const [currentTag, setCurrentTag] = useState('');
    const { sellerId } = useParams();
    const [tagList, setTagList] = useState([]);
    const [newSKU, setNewSKU] = useState("");
    const [formData, setFormData] = useState({
        name: '',
        description: '',
        quantity: '',
        price: '',
        tags: [],
        item: {
            name: '',
            sku: '',
            id: '',
            seller: sellerId

        }
    });
    const [useExistingItem, setUseExistingItem] = useState(false);
    function handleChange(e) {
        e.preventDefault();
        let {name, value} = e.target;
        setFormData({...formData, [name]: value})
    }

    function handleTagChange(e) {
        setCurrentTag(e.target.value);
    }

    function handleTagKeys(e) {
        if(e.key === 'Enter' && currentTag.length > 0 && !/^\s+$/.test(currentTag)) {
            // add tag to form data 
            let updatedTagList = [...formData.tags, currentTag.trim()];
            setFormData({...formData, tags: updatedTagList});
            setTagList(updatedTagList);
            setCurrentTag('');
        }
    }

    function removeTag(e) {
        let index = [...e.target.parentNode.children].indexOf(e.target);
        let updatedTagList = [...tagList.slice(0, index), ...tagList.slice(index + 1, tagList.length)];
        setTagList(updatedTagList);
        setFormData({...formData, tags: updatedTagList});
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
        setFormData({...formData, item:{
            ...formData.item,
            sku
        }})
    }
    return (
        <form id="add-listing-wrapper">
            <h1>Add Listing</h1>
            <input type="file" /> {/* will try to add image input component later */}
            <Input placeholder="Name" required={true} name="name" value={formData.name} onChange={handleChange}/>
            <textarea placeholder="Description" type="textarea" name="description" value={formData.description} onChange={handleChange}/>
            <Input placeholder="Quantity" type="number" name="quantity" value={formData.quantity} onChange={handleChange}/>
            <Input placeholder="Price" name="price" value={formData.price} onChange={handleChange}/>
            <Input placeholder="Tags (Press enter to add a tag)" value={currentTag} name="tags" onKeyDown={handleTagKeys} onChange={handleTagChange}/>            
            <div>
                <TagList tags={tagList} onClick={removeTag}/>
            </div>
            <Input type="checkbox" label="Create listing with existing item?" value={useExistingItem} onChange={e => setUseExistingItem(e.target.checked)}/>
            {useExistingItem ?
                <Input placeholder="Enter existing item SKU" />
                
            : 
                <div id="add-listing-new-item-wrapper">
                    <Input value={formData.item.name} placeholder="name" />
                    <Input value={newSKU} placeholder="Enter new item SKU" />
                    <button onClick={handleSKUButton}>Generate</button>
                </div>
            }
        </form>
    )
}