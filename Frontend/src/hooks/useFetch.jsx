import { useEffect, useState } from 'react';
import axiosInstance from '../utils/axiosInstance.jsx';

const useFetch = (url) => {
    const [data, setData] = useState(null);
    const [isPending, setIsPending] = useState(true);
    const [error, setError] = useState(null);

    useEffect(() => {
        axiosInstance.get(url)
            .then((response) => {
                setData(response.data);
                setIsPending(false);
                setError(null);
            })
            .catch((error) => {
                setIsPending(false);
                setError(error);
            });

        return () => {};
    }, [url]);

    return { data, isPending, error };
};

export default useFetch;
