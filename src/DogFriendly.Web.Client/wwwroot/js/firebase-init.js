
// Initialize Firebase
firebase.initializeApp(firebaseConfig);

// FirebaseUI config
var uiConfig = {
    signInFlow: 'popup',
    signInSuccessUrl: window.location.href,
    signInOptions: [
        firebase.auth.EmailAuthProvider.PROVIDER_ID,
        firebase.auth.GoogleAuthProvider.PROVIDER_ID,
        {
            provider: 'microsoft.com',
            providerName: 'Microsoft',
            fullLabel: 'Connexion Microsoft',
            buttonColor: '#2F2F2F',
            iconUrl: 'img/microsoft.jpeg',
            loginHintKey: 'login_hint'
        },
        firebase.auth.MicrosoftAuthProvider.PROVIDER_ID
    ],
    tosUrl: window.location.href,
    privacyPolicyUrl: window.location.href
};

var ui = new firebaseui.auth.AuthUI(firebase.auth());
ui.start('#firebaseui-auth-container', uiConfig);
