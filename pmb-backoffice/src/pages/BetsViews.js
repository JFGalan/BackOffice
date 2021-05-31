import React, {Component, createRef, Fragment} from "react";
import {Button} from "primereact/button";
import {InputText} from "primereact/inputtext";
import {DataTable} from "primereact/datatable";
import {Column} from "primereact/column";
import axios from "axios";
import {TabPanel, TabView} from "primereact/tabview";
import {Toast} from 'primereact/toast';

export class BetsViews extends Component {

    constructor(props) {
        super(props);
        this.myToast = createRef();
        this.state = {
            betsData: [],
            email: '',
            market: '',
            ev: '',
            makeMarket: '',
            blockMarket: ''
        }
    }

    render() {
        return (
            <Fragment>
                <div className='UserView'>
                    <TabView>
                        <TabPanel header='Filter by email' leftIcon='pi pi-envelope'>
                            <Button icon="pi pi-refresh" className='buttonRefresh' iconPos='left'
                                    onClick={this.refreshBetsView}/>
                            <span className="p-input-icon-left">
                        <i className="pi pi-search"/>
                        <InputText value={this.state.email} className='InputEmail'
                                   onChange={this.changeValueEmailInput} placeholder='Filter by Email'/>
                    </span>
                        </TabPanel>
                        <TabPanel header='Filter by Market' leftIcon='pi pi-shopping-cart'>
                            <Toast ref={this.myToast} position="top-right"></Toast>
                            <Button icon="pi pi-refresh" className='buttonRefresh' iconPos='left'
                                    onClick={this.refreshBetsView}/>
                            <span className="p-input-icon-left">
                                <i className="pi pi-search"/>
                                <InputText value={this.state.market} className='InputMarket'
                                           onChange={this.changeValueMarketInput} placeholder='Filter by Market'/>
                                <Button icon="pi pi-shopping-cart" className='buttonSearchMarket' iconPos='left'
                                        onClick={this.filterByMarket}/>
                            </span>
                        </TabPanel>
                        <TabPanel header='Filter by Event' leftIcon='pi pi pi-fw pi-ticket'>
                            <Toast ref={this.myToast} position="top-right"></Toast>
                            <Button icon="pi pi-refresh" className='buttonRefresh' iconPos='left'
                                    onClick={this.refreshBetsView}/>
                            <span className="p-input-icon-left">
                                <i className="pi pi-search"/>
                                <InputText value={this.state.ev} className='InputMarket'
                                           onChange={this.changeValueEventInput} placeholder='Filter by Event'/>
                                <Button icon="pi pi-fw pi-ticket" className='buttonSearchMarket' iconPos='left'
                                        onClick={this.filterByEvent}/>
                            </span>
                        </TabPanel>
                        <TabPanel header='Insert 3 markets by event' leftIcon='pi pi-plus'>
                            <Toast ref={this.myToast} position="top-right"></Toast>
                            <Button icon="pi pi-refresh" className='buttonRefresh' iconPos='left'
                                    onClick={this.refreshBetsView}/>
                            <span className="p-input-icon-left">
                                <i className="pi pi pi-fw pi-ticket"/>
                                <InputText value={this.state.makeMarket} className='InputMarket'
                                           onChange={this.changeValueInsertMarketInput} placeholder='Insert event'/>
                                <Button icon="pi pi-plus" className='buttonSearchMarket' iconPos='left'
                                        onClick={this.insertMarketByEvent}/>
                            </span>
                        </TabPanel>
                        <TabPanel header='Block Market' leftIcon='pi pi-ban'>
                            <Toast ref={this.myToast} position="top-right"></Toast>
                            <Button icon="pi pi-refresh" className='buttonRefresh' iconPos='left'
                                    onClick={this.refreshBetsView}/>
                            <span className="p-input-icon-left">
                                <i className="pi pi-ban"/>
                                <InputText value={this.state.blockMarket} className='InputBlock'
                                           onChange={this.changeValueBlockMarketInput} placeholder='Block market'/>
                                <Button icon="pi pi-ban" className='buttonSearchMarket' iconPos='left'
                                        onClick={this.blockMarket}/>
                            </span>
                        </TabPanel>
                    </TabView>
                    <div>
                        <DataTable className="p-datatable-gridlines"
                                   value={this.state.betsData}>
                            <Column field="UsuarioId" header="Email"></Column>
                            <Column field="Tipo_Apuesta" header="Bet type"></Column>
                            <Column field="Tipo_Cuota" header="Fee type"></Column>
                            <Column field="Dinero_Apostado" header="Money bet"></Column>
                        </DataTable>
                    </div>
                </div>
            </Fragment>
        );
    }

    componentDidMount() {
        this.getBets();
    }

    getBets = () => {
        axios.get('https://localhost:44355/api/Apuestas').then((resultData) => {
            this.setState({betsData: resultData.data});
            console.log(resultData.data);
        });
    }

    changeValueEmailInput = (event) => {
        this.setState({email: event.target.value}, () => {
            this.filterByEmail();
        });
    }

    filterByEmail = () => {
        axios.get('https://localhost:44355/api/Apuestas?email=' + this.state.email).then((resultData) => {
            this.setState({betsData: resultData.data});
            console.log(resultData.data);
        });
    }

    changeValueMarketInput = (event) => {
        this.setState({market: event.target.value}, () => {
        });
    }

    changeValueEventInput = (event) => {
        this.setState({ev: event.target.value}, () => {
        });
    }

    changeValueInsertMarketInput = (event) => {
        this.setState({makeMarket: event.target.value}, () => {
        });
    }

    changeValueBlockMarketInput = (event) => {
        this.setState({blockMarket: event.target.value}, () => {
        });
    }

    filterByMarket = () => {
        if (this.state.market === '') {
            this.showInfoToast('Empty input', 'Introduce a market for searching it!');
        } else {
            axios.get('https://localhost:44355/api/Apuestas?mercadoId=' + this.state.market).then((resultData) => {
                this.setState({betsData: resultData.data});
                console.log(resultData.data);
            });
        }
    }

    filterByEvent = () => {
        if (this.state.ev === '') {
            this.showInfoToast('Empty input', 'Introduce an event to searching it!');
        } else {
            axios.get('https://localhost:44355/api/Apuestas?eventoId=' + this.state.ev).then((resultData) => {
                this.setState({betsData: resultData.data});
                console.log(resultData.data);
            });
        }

    }

    insertMarketByEvent = () => {
        if (this.state.makeMarket === '') {
            this.showInfoToast('Empty input', 'Introduce an event to make a new 3 markets!');
        } else {
            axios.post('https://localhost:44355/api/Apuestas?eventoId=' + this.state.makeMarket).then((result) => {
                if (result.data === true) {
                    this.showSuccessToast('Insert markets', 'The markets have been inserted.');

                } else {
                    this.showErrorToast('Error', 'The introduced event does not exist!');
                }

            });
        }
    }

    blockMarket = () => {
        if (this.state.blockMarket === '') {
            this.showInfoToast('Empty input', 'You have to insert a market. This input is empty.');
        } else {
            axios.put('https://localhost:44355/api/Mercados?idMercado=' + this.state.blockMarket + '&blockOrUnblock=true').then((result) => {
                if (result.data === 0) {
                    this.showErrorToast('Error', 'The introduced market does not exist.');
                } else if (result.data === 1) {
                    this.showSuccessToast('Blocked', 'The market has been successfully blocked.');
                } else {
                    this.showErrorToast('Error', 'The introduced market was already blocked. Therefore, it cannot be locked again..');
                }
            });
        }
    }

    refreshBetsView = () => {
        this.setState({email: '', market: '', ev: '', makeMarket: '', blockMarket: ''}, () => {
            this.getBets();
        });

    }

    showSuccessToast = (title, order) => {
        this.myToast.current.show({severity: 'success', summary: title, detail: order});

    }

    showErrorToast = (title, order) => {
        this.myToast.current.show({severity: 'error', summary: title, detail: order});

    }

    showInfoToast = (title, order) => {
        this.myToast.current.show({severity: 'info', summary: title, detail: order});

    }
}
