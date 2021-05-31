import './App.css';
import {NavLink, Route, Switch} from "react-router-dom";
import 'primeicons/primeicons.css';
import 'primereact/resources/themes/rhea/theme.css';
import React, {Component, Fragment} from "react";
import 'primereact/resources/primereact.min.css';
import {UsersView} from "./pages/UsersView";
import {BetsViews} from "./pages/BetsViews";
import {EventsView} from "./pages/EventsView";
import {ReportsView} from "./pages/ReportsView";

class App extends Component {

    render() {
        return (
            <Fragment>
                <div className='App'>
                    <div>
                        <ul className={'Menu-Ul-App-Nav'}>
                            <li className={'Il-Menu-Nav-App'}>
                                <NavLink to={'/users'} activeClassName={'MenuActive'}>Users</NavLink>
                            </li>
                            <li className={'Il-Menu-Nav-App'}>
                                <NavLink to={'/bets'} activeClassName={'MenuActive'}>Bets</NavLink>
                            </li>
                            <li className={'Il-Menu-Nav-App'}>
                                <NavLink to={'/events'} activeClassName={'MenuActive'}>Events</NavLink>
                            </li>
                            <li className={'Il-Menu-Nav-App'}>
                                <NavLink to={'/reports'} activeClassName={'MenuActive'}>Reports</NavLink>
                            </li>
                        </ul>
                    </div>
                    <Switch>
                        <Route path={'/users'}>
                            <UsersView></UsersView>
                        </Route>
                        <Route path={'/bets'}>
                            <BetsViews></BetsViews>
                        </Route>
                        <Route path={'/events'}>
                            <EventsView></EventsView>
                        </Route>
                        <Route path={'/reports'}>
                            <ReportsView></ReportsView>
                        </Route>
                    </Switch>
                </div>
            </Fragment>
        );
    }
}

export default App;
