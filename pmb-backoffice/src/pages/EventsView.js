import React, {Component, createRef, Fragment} from "react";
import {Button} from "primereact/button";
import {InputText} from "primereact/inputtext";
import {DataTable} from "primereact/datatable";
import {Column} from "primereact/column";
import axios from "axios";
import {TabPanel, TabView} from "primereact/tabview";
import {Toast} from "primereact/toast";

export class EventsView extends Component {
    constructor(props) {
        super(props);
        this.myToast = createRef();
        this.state = {
            eventsData: [],
            eventTeam: '',
            eventDate: '',
            eventHour: '',
            localTeam: '',
            visitingTeam: '',
            dateMatch: '',
            hourMatch: '',
            deleteEvent: '',
            updateDateEvent: '',
            updateHourEvent: '',
            eventId: '',
        }
    }

    render() {
        return (
            <Fragment>
                <div className='UserView'>
                    <div className='TabV'>
                        <TabView>
                            <TabPanel leftIcon="pi pi-search" header="Filter event by Team">
                                <Toast ref={this.myToast} position="top-right"></Toast>
                                <Button icon="pi pi-refresh" className='buttonRefresh' iconPos='left'
                                        onClick={this.refresh}/>
                                <span className="p-input-icon-left">
                                    <i className="pi pi-search"/>
                                    <InputText className='InputEmail'
                                               value={this.state.eventTeam} placeholder='Filter by Team'
                                               onChange={this.changeValueIdInputSearch}/>
                                    <Button icon="pi pi-fw pi-ticket" className='buttonSearchMarket' iconPos='left'
                                            onClick={this.filterById}/>
                                </span>
                            </TabPanel>
                            <TabPanel leftIcon="pi pi-calendar-times" header="Filter event by Date">
                                <Toast ref={this.myToast} position="top-right"></Toast>
                                <Button icon="pi pi-refresh" className='buttonRefresh' iconPos='left'
                                        onClick={this.refresh}/>
                                <InputText className='InputHour'
                                           value={this.state.eventDate}
                                           placeholder='Enter the date in this format: yy/mm/dd'
                                           onChange={this.changeValueDate}/>
                                <InputText className='InputHour'
                                           value={this.state.eventHour}
                                           placeholder='Enter the time in this format: hh:mm'
                                           onChange={this.changeValueHour}/>
                                <Button icon="pi pi-fw pi-calendar" className='buttonSearchMarket'
                                        iconPos='left'
                                        onClick={this.filterByDateAndHour}/>
                            </TabPanel>
                            <TabPanel leftIcon="pi pi-upload" header="Register an event">
                                <Toast ref={this.myToast} position="top-right"></Toast>
                                <InputText className='InputTeams'
                                           value={this.state.localTeam}
                                           placeholder='Enter Local Team'
                                           onChange={this.changeValueLocalTeam}/>
                                <InputText className='InputTeams'
                                           value={this.state.visitingTeam}
                                           placeholder='Visiting Team'
                                           onChange={this.changeValueVisitingTeam}/>
                                <InputText className='InputTeams'
                                           value={this.state.dateMatch}
                                           placeholder='Date: yy/mm/dd'
                                           onChange={this.changeValueDateMatch}/>
                                <InputText className='InputTeams'
                                           value={this.state.hourMatch}
                                           placeholder='Hour: hh:mm'
                                           onChange={this.changeValueHourMatch}/>
                                <Button icon="pi pi-ticket" className='buttonSearchMarket'
                                        iconPos='left'
                                        onClick={this.registerNewEvents}/>
                            </TabPanel>
                            <TabPanel leftIcon="pi pi-minus" header="Delete an event">
                                <Toast ref={this.myToast} position="top-right"></Toast>
                                <InputText className='InputTeams'
                                           value={this.state.deleteEvent}
                                           placeholder='Introduce ID'
                                           onChange={this.changeValueDeleteEvent}/>
                                <Button icon="pi pi-minus" className='buttonSearchMarket'
                                        iconPos='left'
                                        onClick={this.deleteEvent}/>
                            </TabPanel>
                            <TabPanel leftIcon="pi pi-spin pi-spinner" header="Update event date">
                                <Toast ref={this.myToast} position="top-right"></Toast>
                                <InputText className='InputEmail'
                                           value={this.state.eventId} placeholder='Entry ID'
                                           onChange={this.changeValueEventId}/>
                                <InputText className='InputTeams'
                                           value={this.state.updateDateEvent}
                                           placeholder='Date: yy/mm/dd'
                                           onChange={this.changeValueUpdateDate}/>
                                <InputText className='InputTeams'
                                           value={this.state.updateHourEvent}
                                           placeholder='Hour: hh:mm'
                                           onChange={this.changeValueUpdateHour}/>
                                <Button icon="pi pi-spin pi-spinner" className='buttonSearchMarket'
                                        iconPos='left'
                                        onClick={this.updateEvent}/>
                            </TabPanel>
                        </TabView>
                    </div>
                    <div>
                        <DataTable className="p-datatable-gridlines"
                                   value={this.state.eventsData}>
                            <Column field="EventoId" header="ID"></Column>
                            <Column field="Equipo_Local" header="Local Team"></Column>
                            <Column field="Equipo_Visitante" header="Visiting Team"></Column>
                            <Column field="Dia" header="Date"></Column>
                        </DataTable>
                    </div>
                </div>
            </Fragment>
        );
    }

    componentDidMount() {
        this.getEvents();
    }

    getEvents = () => {
        axios.get('https://localhost:44355/api/Eventos').then((resultData) => {
            this.setState({eventsData: resultData.data});
        });
    }

    changeValueIdInputSearch = (event) => {
        this.setState({eventTeam: event.target.value}, () => {
            console.log(this.state.eventTeam);
        });
    }

    changeValueDate = (event) => {
        this.setState({eventDate: event.target.value}, () => {
            console.log(this.state.eventDate);
        });
    }

    changeValueHour = (event) => {
        this.setState({eventHour: event.target.value}, () => {
            console.log(this.state.eventHour);
        });
    }
    changeValueLocalTeam = (event) => {
        this.setState({localTeam: event.target.value}, () => {
            console.log(this.state.localTeam);
        });
    }

    changeValueVisitingTeam = (event) => {
        this.setState({visitingTeam: event.target.value}, () => {
            console.log(this.state.visitingTeam);
        });
    }

    changeValueDateMatch = (event) => {
        this.setState({dateMatch: event.target.value}, () => {
            console.log(this.state.dateMatch);
        });
    }

    changeValueHourMatch = (event) => {
        this.setState({hourMatch: event.target.value}, () => {
            console.log(this.state.hourMatch);
        });
    }

    changeValueDeleteEvent = (event) => {
        this.setState({deleteEvent: event.target.value}, () => {
            console.log(this.state.deleteEvent);
        });
    }

    changeValueUpdateDate = (event) => {
        this.setState({updateDateEvent: event.target.value}, () => {
            console.log(this.state.updateDateEvent);
        });
    }

    changeValueUpdateHour = (event) => {
        this.setState({updateHourEvent: event.target.value}, () => {
            console.log(this.state.updateHourEvent);
        });
    }

    changeValueEventId = (event) => {
        this.setState({eventId: event.target.value}, () => {
            console.log(this.state.eventId);
        });
    }

    filterById = () => {
        if (this.state.eventTeam === '') {
            this.showInfoToast('Filter Input', 'Introduce an ID for searching it!');
        } else {
            axios.get('https://localhost:44355/api/Eventos?team=' + this.state.eventTeam).then((resultData) => {
                this.setState({eventsData: resultData.data});
                this.emptyInputs();
                console.log(this.state.eventsData.data);
            });
        }
    }

    filterByDateAndHour = () => {
        if (this.state.eventDate === '' || this.state.eventHour === '') {
            this.showInfoToast('Filter Input', 'Introduce a date or hour for searching it!');
        } else {
            axios.get('https://localhost:44355/api/Eventos?dateAndHour=' + this.state.eventDate + " " + this.state.eventHour).then((resultData) => {
                this.setState({eventsData: resultData.data});
                this.emptyInputs();
                console.log(resultData.data);
            });
        }
    }

    registerNewEvents = () => {
        if (this.state.localTeam === '' || this.state.visitingTeam === '' || this.state.dateMatch === '' || this.state.hourMatch === '') {
            this.showInfoToast('Register an event', 'You cannot introduce an event because exist an empty input.');
        } else {
            let newEvent = {
                Equipo_Local: this.state.localTeam,
                Equipo_Visitante: this.state.visitingTeam,
                Dia: this.state.dateMatch + ' ' + this.state.hourMatch
            };
            this.emptyInputs();
            axios.post('https://localhost:44355/api/Eventos', newEvent).then(this.getEvents, this.showSuccessToast('Register an  Event', 'Event has been registered!')).catch((error) => {
                if (error != null) {
                    console.log('Requested Failed');
                }
            });
        }
    }

    deleteEvent = () => {
        if (this.state.deleteEvent === '') {
            this.showInfoToast('Delete an event', 'You cannot delete an event because you do not entry nothing.');
        } else {
            axios.delete('https://localhost:44355/api/Eventos/' + this.state.deleteEvent).then((resultDelete) => {
                if (resultDelete.data === true) {
                    this.showSuccessToast('Delete an  Event', 'Event has been deleted!');
                    this.getEvents();
                    this.emptyInputs();
                } else {
                    this.showErrorToast('Delete an  Event', 'Event has not been deleted, because this do not exist.');
                    this.getEvents();
                    this.emptyInputs();
                }
            }).catch((error) => {
                console.log(error);
            });
        }
    }

    updateEvent = () => {
        if (this.state.updateDateEvent === '' || this.state.updateHourEvent === '' || this.state.eventId === '') {
            this.showInfoToast('Update an event', 'You cannot update an event because you do not entry nothing.');
        } else {
            axios.put('https://localhost:44355/api/Eventos?idEvento=' + this.state.eventId + '&fecha=' + this.state.updateDateEvent + ' ' + this.state.updateHourEvent).then((resultData) => {
                if (resultData.data === true) {
                    this.showSuccessToast('Update an  Event', 'Event has been updated!');
                    this.getEvents();
                    this.emptyInputs();
                } else {
                    this.showErrorToast('Update an  Event an  Event', 'Event has not been updated, because the ID does not exist!');
                    this.getEvents();
                    this.emptyInputs();
                }
            });
        }
    }

    emptyInputs = () => {
        this.setState({
            eventTeam: '',
            eventDate: '',
            eventHour: '',
            localTeam: '',
            visitingTeam: '',
            dateMatch: '',
            hourMatch: '',
            deleteEvent: '',
            updateDateEvent: '',
            updateHourEvent: '',
            eventId: ''
        })
    }

    refresh = () => {
        this.getEvents();
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
