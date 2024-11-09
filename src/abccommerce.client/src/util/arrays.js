export const getInBoundIndex = (arr, i) => {
    // js modulo is actually a remainder
    if(i < 0) {
        return arr.length + (i % arr.length);
    }
    return i % arr.length;
}  

export const getFromRange = (arr, range) => {
    let newArr = [];
    for(let i = range[0]; i !== range[1]; i++) {
        // loop back to start if i is out of array bound
        if(i >= arr.length) {
            if(range[1] === 0) break;
            i = 0;    
        }

        newArr.push(arr[i]);
    }
    if(arr[range[1]]) newArr.push(arr[range[1]]);
    return newArr;
}