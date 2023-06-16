import {useNavigate, useParams} from "react-router-dom";
import {useEffect, useState} from "react";
import {apiEndpoint} from "../../api";
import Typography from "@mui/material/Typography";
import TextField from "@mui/material/TextField";
import Grid from "@mui/material/Grid";
import {formatDate} from "../../imports/text";

const Details = () => {
    const [data, setData] = useState(null);
    const {urlId} = useParams();
    const navigate = useNavigate();

    useEffect(() => {
        apiEndpoint('url').fetchById(urlId)
            .then(res => setData(res.data)).catch(() => navigate('/'));
    }, [navigate, urlId])

    if (!data)
        return (<Typography component={'div'} variant={'h3'}>Loading...</Typography>);

    return (
        <>
            <Typography component={'div'} variant={'h3'} mb={4}>Url Details</Typography>
            <Grid container spacing={3}>
                <Grid item sm={6}>
                    <TextField fullWidth label={'Full Link'} InputProps={{readOnly: true}} variant={'standard'}
                               defaultValue={data['fullUrl']}/>
                </Grid>
                <Grid item sm={6}>
                    <TextField fullWidth label={'Short Link'} InputProps={{readOnly: true}} variant={'standard'}
                               defaultValue={data['shortenedUrl']}/>
                </Grid>
                <Grid item sm={6}>
                    <TextField fullWidth label={'Created By'} InputProps={{readOnly: true}} variant={'standard'}
                               defaultValue={`${data['createdBy'].firstName} ${data['createdBy'].lastName}`}/>
                </Grid>
                <Grid item sm={6}>
                    <TextField fullWidth label={'Created At'} InputProps={{readOnly: true}} variant={'standard'}
                               defaultValue={`${formatDate(data['createdAt'])}`}/>
                </Grid>
                <Grid item sm={6}>
                    <TextField fullWidth label={'Last Updated At'} InputProps={{readOnly: true}} variant={'standard'}
                               defaultValue={`${formatDate(data['lastUpdatedAt'])}`}/>
                </Grid>
            </Grid>
        </>
    )
}

export default Details;