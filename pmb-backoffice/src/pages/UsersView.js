import React, {Component, createRef, Fragment} from "react";
import {InputText} from 'primereact/inputtext';
import {Button} from "primereact/button";
import './Pages.css';
import {Toast} from 'primereact/toast';
import {DataTable} from 'primereact/datatable';
import {Column} from "primereact/column";
import axios from "axios";
import {TabPanel, TabView} from "primereact/tabview";

export class UsersView extends Component {

    constructor(props) {
        super(props);
        this.myToast = createRef();
        this.state = {
            userData: [],
            email: '',
            name: '',
            surname: '',
            deleteEmail: '',
            emailChangePw: '',
            oldPw: '',
            newPw: '',
            confPw: ''
        }
    }

    render() {
        return (
            <Fragment>
                <div className='UserView'>
                    <TabView>
                        <TabPanel leftIcon='pi pi-envelope' header="Filter by email">
                            <Button icon='pi pi-refresh' className='buttonDelete' iconPos='left'
                                    onClick={this.refreshUserView}/>
                            <span className="p-input-icon-left">
                                <i className="pi pi-search"/>
                                 <InputText value={this.state.email} className='InputEmail'
                                            onChange={this.changeValueEmailInput} placeholder='Filter by Email'/>
                            </span>
                        </TabPanel>
                        <TabPanel leftIcon='pi pi-search-plus' header='Filter by name'>
                            <Button icon='pi pi-refresh' className='buttonDelete' iconPos='left'
                                    onClick={this.refreshUserView}/>
                            <span className="p-input-icon-left">
                                <i className="pi pi-search"/>
                                <InputText value={this.state.name} className='InputName'
                                           onChange={this.changeValueNameInput} placeholder='Filter by Name'/>
                            </span>
                        </TabPanel>
                        <TabPanel leftIcon='pi pi-search-plus' header='Filter by surname'>
                            <Button icon='pi pi-refresh' className='buttonDelete' iconPos='left'
                                    onClick={this.refreshUserView}/>
                            <span className="p-input-icon-left">
                                <i className="pi pi-search"/>
                                <InputText value={this.state.surname} className='InputSurname'
                                           onChange={this.changeValueSurNameInput} placeholder='Filter by Surname'/>
                            </span>
                        </TabPanel>
                        <TabPanel leftIcon='pi pi-trash' header='Delete by email'>
                            <Toast ref={this.myToast} position="top-right"></Toast>
                            <Button icon='pi pi-refresh' className='buttonDelete' iconPos='left'
                                    onClick={this.refreshUserView}/>
                            <span className="p-input-icon-left">
                                <i className="pi pi-trash"/>
                                <InputText value={this.state.deleteEmail} className='InputDelete'
                                           placeholder='Delete by email'
                                           onChange={this.changeValueDeleteInput}/>
                               <Button icon='pi pi-trash' className='buttonDelete' iconPos='left'
                                       onClick={this.deleteByEmail}/>
                            </span>
                        </TabPanel>
                        <TabPanel leftIcon='pi pi-key' header='New Password'>
                            <Toast ref={this.myToast} position="top-right"></Toast>
                            <Button icon='pi pi-refresh' className='buttonDelete' iconPos='left'
                                    onClick={this.refreshUserView}/>
                            <span className="p-input-icon-left">
                                <InputText value={this.state.emailChangePw} className='InputPassword'
                                           placeholder='Email user' onChange={this.changeValueEmailChange}/>
                                <InputText value={this.state.oldPw} className='InputPassword' placeholder='Old Password'
                                           onChange={this.changeValueOldPwChange}/>
                                <InputText value={this.state.newPw} className='InputPassword' placeholder='New Password'
                                           onChange={this.changeValueNewPwChange}/>
                                <InputText value={this.state.confPw} className='InputPassword2'
                                           placeholder='Confirm Password' onChange={this.changeValueConfPwChange}/>
                                <Button icon='pi pi-key' className='buttonDelete' iconPos='left'
                                        onClick={this.setNewPassword}/>
                            </span>
                        </TabPanel>
                    </TabView>
                    <div>
                        <DataTable className="p-datatable-gridlines"
                                   value={this.state.userData}>
                            <Column field="EmailId" header="Email"></Column>
                            <Column field="Nombre" header="Name"></Column>
                            <Column field="Apellidos" header="Surname"></Column>
                            <Column field="Edad" header="Age"></Column>
                        </DataTable>
                    </div>
                </div>
            </Fragment>
        );
    }

    componentDidMount() {
        this.getUsers();
    }

    getUsers = () => {
        axios.get('https://localhost:44355/api/Usuarios').then((resultData) => {
            this.setState({userData: resultData.data});
        }).catch(error => {
            console.log(error);
        });
    }

    changeValueEmailInput = (event) => {
        this.setState({email: event.target.value}, () => {
            this.filterByEmail();
        });
    }

    changeValueNameInput = (event) => {
        this.setState({name: event.target.value}, () => {
            this.filterByName();
        });
    }

    changeValueSurNameInput = (event) => {
        this.setState({surname: event.target.value}, () => {
            this.filterBySurname();
        });
    }

    changeValueDeleteInput = (event) => {
        this.setState({deleteEmail: event.target.value}, () => {
        });
    }

    changeValueEmailChange = (event) => {
        this.setState({emailChangePw: event.target.value}, () => {
        });
    }

    changeValueOldPwChange = (event) => {
        this.setState({oldPw: event.target.value}, () => {
        });
    }

    changeValueNewPwChange = (event) => {
        this.setState({newPw: event.target.value}, () => {
        });
    }

    changeValueConfPwChange = (event) => {
        this.setState({confPw: event.target.value}, () => {
        });
    }

    filterByEmail = () => {
        axios.get('https://localhost:44355/api/Usuarios?email=' + this.state.email).then((resultData) => {
            let arrayUser = [];
            arrayUser.push(resultData.data);
            this.setState({userData: arrayUser});
        }).catch(error => {
            console.log(error);
        });
    }

    filterByName = () => {
        axios.get('https://localhost:44355/api/Usuarios?name=' + this.state.name).then((resultData) => {
            let arrayUser = [];
            arrayUser.push(resultData.data);
            this.setState({userData: arrayUser});
        }).catch(error => {
            console.log(error);
        });
    }

    filterBySurname = () => {
        axios.get('https://localhost:44355/api/Usuarios?surname=' + this.state.surname).then((resultData) => {
            let arrayUser = [];
            arrayUser.push(resultData.data);
            this.setState({userData: arrayUser});
        }).catch(error => {
            console.log(error);
        });
    }

    deleteByEmail = () => {
        if (this.state.deleteEmail === '') {
            this.showInfoToast('Delete Input', 'The delete input is empty and therefore no action is taken.');
        } else {
            axios.delete('https://localhost:44355/api/Usuarios?emailId=' + this.state.deleteEmail).then((resultData) => {

                if (resultData.data === true) {
                    this.showSuccessToast('Delete Email', 'The email has been deleted.');
                } else {
                    this.showErrorToast('Impossible delete an Email', 'The user could not be deleted, because it does not exist!');
                }
                this.refreshUserView();
            }).catch(error => {
                console.log(error);
            });
        }

    }

    setNewPassword = () => {
        if (this.state.emailChangePw === '' || this.state.oldPw === '' || this.state.newPw === '' || this.state.confPw === '') {
            this.showInfoToast('Empty Input', 'There is an empty input and therefore no action has been taken. Check it out.');
        } else {
            let password = {
                OldPassword: this.state.oldPw,
                NewPassword: this.state.newPw,
                ConfirmPassword: this.state.confPw
            }
            axios.post('https://localhost:44355/api/Account/ChangeNewPassword?email=' + this.state.emailChangePw, password).then((result) => {
                if (result != null) {
                    this.showSuccessToast('Password Changed', 'Password has been changed!');
                }
            }).catch(error => {
                console.log(error);
            });
        }
    }

    refreshUserView = () => {
        this.setState({
            email: '',
            name: '',
            surname: '',
            deleteEmail: '',
            emailChangePw: '',
            oldPw: '',
            newPw: '',
            confPw: ''
        }, () => {
            this.getUsers();
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

