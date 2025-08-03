//this file javacript code for chat page
// chat.js - ملف JavaScript الكامل للشات البسيط

// استيراد مكتبات Firebase
import { initializeApp } from "https://www.gstatic.com/firebasejs/9.6.0/firebase-app.js";
import { getDatabase, ref, push, onChildAdded } from "https://www.gstatic.com/firebasejs/9.6.0/firebase-database.js";

// تكوين Firebase
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

// تهيئة Firebase
const app = initializeApp(firebaseConfig);
const database = getDatabase(app);

// متغيرات عامة
let currentUserId = null;
let otherUserId = null;
let chatId = null;

// تهيئة الشات
function initChat(currentUser, otherUser) {
    currentUserId = currentUser;
    otherUserId = otherUser;

    // إنشاء معرف المحادثة الفريد (ترتيب الأرقام للتأكد من أنه نفس الـ ID لكلا المستخدمين)
    chatId = currentUserId < otherUserId ?
        `${currentUserId}_${otherUserId}` :
        `${otherUserId}_${currentUserId}`;

    setupEventListeners();
    listenForMessages();
}

// إعداد مستمعي الأحداث
function setupEventListeners() {
    // زر الإرسال
    document.getElementById('sendButton').addEventListener('click', sendMessage);

    // إرسال بالضغط على Enter
    document.getElementById('messageInput').addEventListener('keypress', (e) => {
        if (e.key === 'Enter') {
            sendMessage();
        }
    });
}

// إرسال رسالة
function sendMessage() {
    const input = document.getElementById('messageInput');
    const messageText = input.value.trim();

    if (messageText && chatId) {
        // إنشاء كائن الرسالة
        const message = {
            text: messageText,
            senderId: currentUserId,
            timestamp: Date.now()
        };

        // إرسال الرسالة إلى Firebase
        push(ref(database, `chats/${chatId}/messages`), message)
            .then(() => {
                input.value = ''; // مسح حقل الإدخال بعد الإرسال
                input.focus(); // إعادة التركيز على حقل الإدخال
            })
            .catch((error) => {
                console.error('Error sending message:', error);
            });
    }
}

// الاستماع للرسائل الجديدة
function listenForMessages() {
    onChildAdded(ref(database, `chats/${chatId}/messages`), (snapshot) => {
        const message = snapshot.val();
        displayMessage(message);
    });
}

// عرض الرسالة في الواجهة
function displayMessage(message) {
    const container = document.getElementById('messagesContainer');
    const isCurrentUser = message.senderId === currentUserId;

    const messageDiv = document.createElement('div');
    messageDiv.className = `message ${isCurrentUser ? 'sent' : 'received'}`;

    messageDiv.innerHTML = `
        <div class="message-text">${message.text}</div>
        <div class="message-time">${formatTime(message.timestamp)}</div>
    `;

    container.appendChild(messageDiv);
    container.scrollTop = container.scrollHeight; // التمرير لآخر رسالة
}

// تنسيق الوقت
function formatTime(timestamp) {
    const date = new Date(timestamp);
    return date.toLocaleTimeString([], {
        hour: '2-digit',
        minute: '2-digit',
        hour12: true
    });
}

// تصدير الدالة الرئيسية للتهيئة
export function setupChat(currentUser, otherUser) {
    initChat(currentUser, otherUser);
}