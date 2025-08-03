import { initializeApp } from 'https://www.gstatic.com/firebasejs/9.6.0/firebase-app.js';
import { getDatabase } from 'https://www.gstatic.com/firebasejs/9.6.0/firebase-database.js';
import { getAuth } from 'https://www.gstatic.com/firebasejs/9.6.0/firebase-auth.js';

const firebaseConfig = {
    apiKey: "AIzaSyCgrn9MIAa3J6TL-KW_gWbmMw3hz_qL7t0",
    authDomain: "doctor-chat-app-5df92.firebaseapp.com",
    databaseURL: "https://doctor-chat-app-5df92-default-rtdb.firebaseio.com",
    projectId: "doctor-chat-app-5df92",
    storageBucket: "doctor-chat-app-5df92.appspot.com",
    messagingSenderId: "319942931151",
    appId: "1:319942931151:web:92ce7433148765adf63dba",
    measurementId: "G-R5PMNV2LB3"
};

const app = initializeApp(firebaseConfig);
const database = getDatabase(app);
const auth = getAuth(app);

export { database, auth };