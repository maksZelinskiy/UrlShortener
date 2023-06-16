import React, {useEffect, useState} from 'react';
import {apiEndpoint} from "../../api";
import Button from "@mui/material/Button";
import Typography from "@mui/material/Typography";
import UrlModal from "../../components/urlModal/UrlModal";
import styles from './urlTable.module.css';
import {formatDate, messageTypes} from "../../imports/text";
import {launchError, launchToast} from "../../components/layout/Layout";
import {useNavigate} from "react-router-dom";

const UrlsTable = () => {
    const [state, setState] = useState({urls: [], loading: true, canAdd: false});
    const [open, setOpen] = useState(false);
    const navigate = useNavigate();

    useEffect(() => {
        apiEndpoint('url')
            .fetch()
            .then(res => setState({urls: res.data.urls, loading: false, canAdd: res.data.canAdd}));
    }, []);

    const handleClick = (url, canEdit) => {
        if (!canEdit)
            return;
        setOpen(url);
    }

    const remove = (id) => {
        apiEndpoint('url')
            .deleteById(id)
            .then(() => {
                setState({...state, urls: state.urls.filter(u => u.url.id !== id)});
                launchToast('Url was removed', messageTypes.success);
            })
            .catch(err => launchError(err))
    }

    const handleShort = (url) => {
        navigator.clipboard.writeText(url)
            .then(() => launchToast('Copied!', messageTypes.success));
    }

    const renderForecastsTable = (urls) => {
        return (
            <table className="table table-striped" aria-labelledby="tableLabel">
                <thead>
                <tr>
                    <th>Full Url</th>
                    <th>Short Url <i>(Click to copy)</i></th>
                    <th>Created At</th>
                    <th>Last Updated At</th>
                    {state.canAdd && <>
                        <th width={10}></th>
                        <th width={10}></th>
                        <th width={10}></th>
                    </>}
                </tr>
                </thead>
                <tbody>
                {urls.map(urlWrap =>
                    <tr key={urlWrap.url.id + 'Url'} className={styles.row}>
                        <td>{urlWrap.url['fullUrl']}</td>
                        <td onClick={() => handleShort(urlWrap.url['shortenedUrl'])}>
                            {urlWrap.url['shortenedUrl']}
                        </td>
                        <td>{formatDate(urlWrap.url['createdAt'])}</td>
                        <td>{formatDate(urlWrap.url['lastUpdatedAt'])}</td>
                        {state.canAdd &&
                            <td className={styles.edit}>
                                <ion-icon name="navigate-outline"
                                          onClick={() => navigate('/details/' + urlWrap.url.id)}></ion-icon>
                            </td>
                        }
                        {urlWrap['canEdit'] &&
                            <>
                                <td className={styles.edit}>
                                    <ion-icon name="create-outline"
                                              onClick={() => handleClick(urlWrap.url, urlWrap['canEdit'])}></ion-icon>
                                </td>
                                <td className={styles.remove}>
                                    <ion-icon name="trash-outline"
                                              onClick={() => remove(urlWrap.url.id)}></ion-icon>
                                </td>
                            </>
                        }
                    </tr>
                )}
                </tbody>
            </table>
        );
    }

    const content = state.loading
        ? <p><em>Loading...</em></p>
        : renderForecastsTable(state.urls);

    return (
        <div>
            <Typography component={'h1'} variant={'h3'}>Urls Table</Typography>
            {state.canAdd &&
                <Button variant={'contained'} onClick={() => setOpen(true)} style={{margin: '15px 0'}}>
                    Add Url
                </Button>}
            <UrlModal open={open} setOpen={setOpen} setState={setState} state={state}/>
            {content}
        </div>
    );
}

export default UrlsTable;