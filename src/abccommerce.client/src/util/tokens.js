export async function setUserToken(tokenData) {
    let expDate = new Date(tokenData.expirationDate);
    document.cookie = `userToken=${tokenData.token}; expires=${expDate}; path=/`;
} 

export async function setRefreshToken(tokenData) {
    const refreshTokenMonths = 6;
    let expDate = new Date(Date.now() + refreshTokenMonths * 30 * 24 * 60 * 60 * 1000);
    document.cookie = `refreshToken=${tokenData.refreshToken}; expires=${expDate}; path=/`;
}

export async function setTokens(tokenData) {
    setUserToken(tokenData);
    setRefreshToken(tokenData);
}

export async function refresh(refreshToken) {
    if(!refreshToken) return false;
    try {
        let response = await fetch('http://localhost:5147/user/refresh', {
            method: 'POST',
            headers: {
                "Content-Type": "application/json"
            },
            body: JSON.stringify({
                refreshToken
            })
        })

        if(!response.ok) return false;

        let tokenData = await response.json();
        setUserToken(tokenData);
        return true;
    }
    catch(error) {
        console.error(error);
        return false;
    }
}
