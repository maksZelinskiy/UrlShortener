import About from "./pages/about/About";
import UrlsTable from "./pages/urlsTable/UrlsTable";
import Login from "./pages/login/Login";
import SignUp from "./pages/signUp/SignUp";
import Details from "./pages/details/Details";

const AppRoutes = [
    {
        index: true,
        element: <About/>
    },
    {
        path: '/details/:urlId',
        element: <Details/>
    },
    {
        path: '/table',
        element: <UrlsTable/>
    },
    {
        path: '/sign-in',
        element: <Login/>
    },
    {
        path: '/sign-up',
        element: <SignUp/>
    }
];

export default AppRoutes;
