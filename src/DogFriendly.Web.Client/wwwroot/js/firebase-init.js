var fb;

window.getFirebaseAuthToken = async function () {
    if (fb == null)
        fb = firebase.initializeApp(firebaseConfig);

    const user = fb.auth().currentUser;
    if (user) {
        return await user.getIdToken();
    }

    return null;
};

window.isFirebaseUserAuth = async function () {
    return (await window.getFirebaseAuthToken()) !== null;
}

window.updateFirebaseAuth = async function () {
    if (fb == null) {
        fb = firebase.initializeApp(firebaseConfig);
        fb.auth().onAuthStateChanged(async function (user) {
            if (user == null) return;

            if (!user.emailVerified) {
                await user.sendEmailVerification();
            } else {
                var token = await user.getIdToken();
                await DotNet.invokeMethodAsync('DogFriendly.Web.Client', 'SetJwtToken', token);
            }
        }, function (error) {
            console.error(error);
        });
    }
};

window.initFirebaseUi = async function () {
    await window.updateFirebaseAuth();
    if ($('#firebaseui-auth-container').length > 0
        && !(await window.isFirebaseUserAuth())) {
        try {
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
                callbacks: {
                    signInSuccessWithAuthResult: function () {
                        window.location.reload();
                        return false;
                    }
                }
            };
            new firebaseui.auth.AuthUI(fb.auth()).start('#firebaseui-auth-container', uiConfig);
        } catch { }
    }
}

window.logoutFirebaseAuth = async function () {
    if (fb != null) {
        fb.auth().signOut();
        window.location.reload();
    }
}