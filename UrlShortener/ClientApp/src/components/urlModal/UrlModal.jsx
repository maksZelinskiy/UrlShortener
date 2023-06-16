import Box from '@mui/material/Box';
import Button from '@mui/material/Button';
import Typography from '@mui/material/Typography';
import Modal from '@mui/material/Modal';
import TextField from "@mui/material/TextField";
import {apiEndpoint} from "../../api";
import {launchError, launchToast} from "../layout/Layout";
import {useEffect, useState} from "react";
import {messageTypes} from "../../imports/text";

const style = {
    position: 'absolute',
    display: 'flex',
    flexDirection: 'column',
    gap: '20px',
    top: '50%',
    left: '50%',
    transform: 'translate(-50%, -50%)',
    minWidth: 400,
    bgcolor: 'background.paper',
    border: '2px solid #000',
    borderRadius: '10px',
    boxShadow: 24,
    p: 4,
};

const UrlModal = ({open, setOpen, setState, state}) => {
    const [url, setUrl] = useState(open['fullUrl'] ?? '');
    const isEdit = !!open.id;

    useEffect(() => {
        setUrl(open['fullUrl'] ?? '');
    }, [open])

    const addUrl = () => {
        apiEndpoint('url')
            .post(url)
            .then(res => {
                console.log(res.data);
                setState({...state, urls: state.urls.concat({url: res.data, canEdit: true})});
                setOpen(false);
                launchToast('Url added', messageTypes.success);
            })
            .catch(err => launchError(err));
    }

    const editUrl = () => {
        apiEndpoint('url/' + open.id)
            .put(url)
            .then(res => {
                let url = state.urls.filter(u => u.url.id === res.data.id)[0];
                url.url = res.data;
                setState(state);
                setOpen(false);
                launchToast('Url edited', messageTypes.success);
            })
            .catch(err => launchError(err));
    }

    if (!open)
        return (<></>);

    return (
        <>
            <Modal
                open={!!open}
                onClose={() => setOpen(false)}
                aria-labelledby="modal-modal-title"
                aria-describedby="modal-modal-description"
            >
                <Box sx={style}>
                    <Typography id="modal-modal-title" variant="h5" component="h2">
                        {isEdit ? 'Edit Url' : 'Enter New Url'}
                    </Typography>
                    <TextField fullWidth name={'url'} label={'Url'} autoFocus
                               value={url} onChange={(event) => setUrl(event.target.value)}/>
                    <div>
                        {isEdit ? <Button onClick={editUrl}>Edit</Button> : <Button onClick={addUrl}>Add</Button>}
                    </div>
                </Box>
            </Modal>
        </>
    )
}

export default UrlModal;