import React, {useEffect, useState} from 'react';
import {Collapse, Navbar, NavbarBrand, NavbarToggler, NavItem, NavLink} from 'reactstrap';
import {Link, useNavigate} from 'react-router-dom';
import './NavMenu.css';
import {logout} from "../../imports/text";

const pages = [
    {name: 'Urls Table', link: '/table'}
]

const NavMenu = () => {
    const [collapsed, setCollapsed] = useState(true);
    const [logged, setLogged] = useState(false);
    const toggleNavbar = () => setCollapsed(!collapsed);
    const navigate = useNavigate();

    useEffect(() => {
        setLogged(!!localStorage.getItem('bearer_token'));
    }, [navigate])

    return (
        <header>
            <Navbar className="navbar-expand-sm navbar-toggleable-sm ng-white border-bottom box-shadow mb-3"
                    container light>
                <NavbarBrand tag={Link} to="/">Url Shortener</NavbarBrand>
                <NavbarToggler onClick={toggleNavbar} className="mr-2"/>
                <Collapse className="d-sm-inline-flex flex-sm-row-reverse" isOpen={!collapsed} navbar>
                    <ul className="navbar-nav flex-grow">
                        {pages.map(page =>
                            <NavItem key={page.name}>
                                <NavLink tag={Link} className="text-dark" to={`${page.link}`}>
                                    {page.name}
                                </NavLink>
                            </NavItem>)}
                        {logged ? <NavItem>
                            <NavLink tag={Link} className="text-dark" to={`/sign-in`} onClick={() => logout()}>
                                Logout
                            </NavLink>
                        </NavItem> : <NavItem>
                            <NavLink tag={Link} className="text-dark" to={`/sign-in`}>
                                Login
                            </NavLink>
                        </NavItem>}
                    </ul>
                </Collapse>
            </Navbar>
        </header>
    );
}

export default NavMenu;