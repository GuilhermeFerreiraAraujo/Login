'use client';

import styles from './page.module.css';
import axios from 'axios';
import { useState } from 'react';

const domain = 'https://localhost:7094/';

var headers = {
    'Access-Control-Allow-Origin': '*',
    'Access-Control-Allow-Headers': '*',
    'Access-Control-Allow-Credentials': 'true'
}

export function Post(url, data) {
  const endpoint = `${domain}${url}`
  return axios.post(endpoint, data, headers);
}

export default function Home() {

  const [jwt, setJwt] = useState('');
  
  const login = () =>{
    const data = {
      username: 'guilherme', 
      password: 'derteufel'
    }

    Post('Login/Login', data).then(response => {
      setJwt(`Bearer ${response.data}`)
    }).catch(ex => {
      console.log('There was an issue with your request.');
    });

  }
  
  const isLoggedIn = () => {
    axios.defaults.headers.common["Authorization"] = jwt;

    axios.get(`${domain}${'Login/IsLoggedIn'}`, headers).then(response => {
      alert('It is logged in');
    }).catch(ex => {
      alert('It is not logged in');
    })
  }

  const logout = () => {
    setJwt('');
  }

  return (
    <main className={styles.main}>
      <button onClick={() => login()}>Login</button>
      <button onClick={() => isLoggedIn()}>Test login</button>
      <button onClick={() => logout()}>Logout</button>
    </main>
  )
}
