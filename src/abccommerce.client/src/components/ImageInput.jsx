import { useState } from 'react';
import './ImageInput.css';
import ImageUpload from './ImageUpload';
import plusSymbol from '../assets/plus-symbol.svg';
import { toBase64 } from '../util/files';
import { validateFile } from '../util/validation';
/**
 * @typedef {Object} ImageFile
 * @prop {String} encoding Base64 encoding of image
 * @param {String} extension Type of image (.png, .jpg, etc.) 
 */

/**
 * @category component
 * Provides drag and drop / file picking functionality for images. Images uploaded are displayed along with their names  
 * and file sizes. Images can be removed from input.
 * @function ImageInput
 * @author Thomas Scott
 * @since December 1
 * @param {Object} props
 * @param {Array<ImageFile>} images Image file list to be changed on input
 * @param {Function} handleImageInput Function to be called on input 
 * @returns {JSX.Element}
 */
export default function ImageInput({images, handleImageInput=()=>{}}) {
    const [imageUploads, setImageUploads] = useState([]);
    const [dragging, setDragging] = useState(false);
    const [errors, setErrors] = useState('');
    const typesAccepted = ["png", "jpg", "jpeg"]
    function handleDrop(e) {
        e.preventDefault();
        e.stopPropagation();
        setDragging(false);
        handleImages(e.dataTransfer.files);

    }

    function handleDrag(e) {
        e.preventDefault();
        e.stopPropagation();
        if(e.type === 'dragenter' || e.type === 'dragover') {
            setDragging(true);
        } else if(e.type === 'dragleave') {
            setDragging(false);
        }
    }

    async function handleChange(e) {
        e.preventDefault();
        handleImages(e.target.files);
    }

    function handleDelete(image) {
        let index = imageUploads.indexOf(image);
        setImageUploads([...imageUploads.slice(0, index), ...imageUploads.slice(index + 1, imageUploads.length)]);
        handleImageInput([...images.slice(0, index), ... images.slice(index + 1, images.length)]);
    }

    async function handleImages(files) {
        setErrors('');
        let imageUploadObjects = [];
        let imageFileObjects = [];
        let filesIncorrect = 0;
        for(const file of files) {
            // validate file types
            if(!validateFile(typesAccepted, file)) {
                filesIncorrect++;
                continue;
            }

            // create object urls to be used in ImageUpload
            let previewURL = URL.createObjectURL(file);
            imageUploadObjects.push({
                name: file.name,
                size: Math.floor(file.size / 1000).toString().concat(' KB'),
                previewURL
            })
            
            // create ImageFile objects with base64 encoding of file
            let encoding = await toBase64(file);
            imageFileObjects.push({
                encoding,
                extension: ".".concat(file.type.slice(file.type.indexOf('/') + 1))
            })
        }
        if(filesIncorrect > 0) {
            setErrors([`${filesIncorrect} of your uploaded files are of the incorrect type`,
                         `Types accepted: ${typesAccepted.toString()}`]);
        }
        setImageUploads([...imageUploads, ...imageUploadObjects]);
        handleImageInput([...images, ...imageFileObjects])
    }

    return (
        <>
        <div className="image-input-wrapper" >
            <label htmlFor='image-input' style={{
            height: !imageUploads || !imageUploads[0] ? '400px' : 'auto',
            border: dragging ? '2px dotted var(--secondary-border-color)' : '2px solid var(--main-bg-color)',
            backgroundColor: dragging && !imageUploads[0] ? 'var(--secondary-input-bg-color)' : ''
        }}>
                {imageUploads && imageUploads[0] ?
                <div className="image-input-with-file-wrapper">
                    <table>
                        <thead>
                            <tr>
                                <th scope="col" sytle={{width: "33%"}}>
                                    Preview
                                </th>
                                <th scope="col" style={{width: "33%"}}>
                                    Name
                                </th>
                                <th scope="col" style={{width: "20%"}}>
                                    Size
                                </th>
                                <th scope="col" style={{width: "14%"}}>
                                    
                                </th>
                            </tr>
                        </thead>
                        <tbody>
                            {imageUploads.map(i =>
                                <ImageUpload {...i} onDelete={() => handleDelete(i)} key={i.previewURL}/>
                            )}
                        </tbody>
                    </table>
                    <label htmlFor="image-input-with-files">
                        <input className="hidden"
                        id="image-input-with-files"
                        type="file"
                        multiple
                        accept="image/jpeg, image/jpg, image/png"
                        onChange={handleChange}
                        />
                        <div
                        className='dropzone'
                        onDrop={handleDrop}
                        onDragEnter={handleDrag}
                        onDragLeave={handleDrag}
                        onDragOver={handleDrag}
                        >
                        </div>
                        <img src={plusSymbol} />
                    </label>
                </div>
                :
                <>
                <input className="hidden"
                id="image-input"
                type="file"
                multiple
                accept="image/jpeg, image/jpg, image/png"
                onChange={handleChange}
                />
                <div
                className='dropzone'
                onDrop={handleDrop}
                onDragEnter={handleDrag}
                onDragLeave={handleDrag}
                onDragOver={handleDrag}
                >
                </div>
                <img src={plusSymbol} />
                <p>Add image</p>
                </>
                }
            </label>
        </div>
        {errors &&
            errors.map((err, i) =>
                <p className="error" key={i}>{err}</p>
            ) 
        }
        </>
    )
}