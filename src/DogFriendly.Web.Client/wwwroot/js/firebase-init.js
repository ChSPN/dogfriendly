window.getFirebaseAuthToken = async function () {
    try {
        firebase.initializeApp(firebaseConfig);
    } catch { }

    const user = firebase.auth().currentUser;
    if (user) {
        return await user.getIdToken();
    }

    return null;
};

window.updateFirebaseAuth = function () {
    try {
        firebase.initializeApp(firebaseConfig);
        firebase.auth().onAuthStateChanged(function (user) {
            if (user) {
                $('[data-isUserAuth="true"]').show();
                $('[data-isUserAuth="false"]').hide();
                $('.logout').show();
                $('#firebaseui-auth-container').hide();
                $('[data-userName]').show();
                $('[data-userName] strong').text(user.displayName);
            } else {
                $('[data-isUserAuth="false"]').show();
                $('[data-isUserAuth="true"]').hide();
                $('.logout').hide();
                $('#firebaseui-auth-container').show();
                $('[data-userName]').hide();
            }
        }, function (error) {
            console.error(error);
        });
    } catch { }
};

window.initFirebaseUi = function () {
    if ($('#firebaseui-auth-container').length > 0) {
        try {
            $('.logout').hide();
            window.updateFirebaseAuth();
            var uiConfig = {
                signInFlow: 'popup',
                signInSuccessUrl: window.location.href,
                signInOptions: [
                    firebase.auth.EmailAuthProvider.PROVIDER_ID,
                    firebase.auth.GoogleAuthProvider.PROVIDER_ID,
                    {
                        provider: 'microsoft.com',
                        providerName: 'Microsoft',
                        fullLabel: 'Se connecter avec Microsoft',
                        buttonColor: '#2F2F2F',
                        iconUrl: 'img/microsoft.jpeg',
                        loginHintKey: 'login_hint'
                    },
                    firebase.auth.FacebookAuthProvider.PROVIDER_ID,
                ],
                tosUrl: window.location.href,
                privacyPolicyUrl: window.location.href,
                language: 'fr'
            };
            new firebaseui.auth.AuthUI(firebase.auth()).start('#firebaseui-auth-container', uiConfig);
        } catch { }
    }
}

window.logoutFirebaseAuth = function () {
    try {
        firebase.initializeApp(firebaseConfig);
    } catch { }

    firebase.auth().signOut();
    window.updateFirebaseAuth();
}

window.updateFirebaseAuth();
window.initFirebaseUi();