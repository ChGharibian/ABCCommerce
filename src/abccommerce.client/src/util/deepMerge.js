

export default function deepMerge(obj1, obj2) {
  const result = {};

    for (const key in obj1) {
        if (obj1.hasOwnProperty(key)) {
            if (
                obj1[key] &&
                typeof obj1[key] === 'object' &&
                !Array.isArray(obj1[key])
            ) {
                result[key] = deepMerge(
                    obj1[key],
                    obj2[key] && typeof obj2[key] === 'object' ? obj2[key] : {}
                );
            } else {
                result[key] = obj1[key];
            }
        }
    }

    for (const key in obj2) {
        if (obj2.hasOwnProperty(key) && !result.hasOwnProperty(key)) {
            result[key] = obj2[key];
        }
    }

    return result;
}