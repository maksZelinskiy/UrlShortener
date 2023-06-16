import React, {useEffect, useState} from 'react';
import Typography from "@mui/material/Typography";
import Button from "@mui/material/Button";
import {apiEndpoint} from "../../api";
import TextField from "@mui/material/TextField";
import {launchError, launchSuccess} from "../../components/layout/Layout";

const About = () => {
    const [edit, setEdit] = useState(false);
    const [data, setData] = useState({about: {id: null, topic: '', content: ''}, canEdit: false});

    useEffect(() => {
        apiEndpoint('about').fetch().then(res => setData(res.data));
    }, [])

    const handleSubmit = () => {
        apiEndpoint('about').put(data.about)
            .then(res => {
                setEdit(false);
                launchSuccess(res);
            }).catch(err => launchError(err));
    }

    if (data.about.id === null)
        return (<Typography component={'div'} variant={'h3'}>Loading...</Typography>)

    if (edit)
        return (
            <>
                <TextField fullWidth label={'Topic'} value={data.about.topic} variant={'standard'}
                           onChange={(event) => setData({
                               ...data,
                               about: {...data.about, topic: event.target.value}
                           })}/>

                <div style={{margin: '30px 0'}}></div>

                <TextField fullWidth multiline label={'Content'} value={data.about.content} variant={'standard'}
                           onChange={(event) => setData({
                               ...data,
                               about: {...data.about, content: event.target.value}
                           })}/>

                <div style={{display: 'flex', justifyContent: 'center', margin: '30px 0', gap: '20px'}}>
                    <Button size={'large'} variant={'contained'} onClick={handleSubmit}>
                        Save
                    </Button>
                    <Button size={'large'} variant={'outlined'} onClick={() => setEdit(false)}>
                        Return
                    </Button>
                </div>
            </>
        )
    return (
        <>
            <Typography component={'div'} variant={'h3'} mb={5}>{data.about.topic}</Typography>
            <Typography component={'div'} variant={'subtitle1'} mb={5}>{data.about.content}</Typography>
            {data.canEdit && <div style={{display: 'flex', justifyContent: 'center'}}>
                <Button size={'large'} variant={'outlined'} onClick={() => setEdit(true)}>
                    Edit
                </Button>
            </div>}
        </>
    )
}

export default About;
