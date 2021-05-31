import {Component, Fragment} from "react";
import {Chart} from "primereact/chart";
import axios from "axios";
import {TabPanel, TabView} from "primereact/tabview";


export class ReportsView extends Component {

    constructor(props) {
        super(props);
        this.state = {
            labelsData: [],
            infoBetData: [],
            labelsDataUsers:[],
            infoUserData:[]
        }
    }

    render() {

        const data = {
            labels: this.state.labelsData,
            datasets: [
                {
                    label: 'Quantity of bets',
                    data: this.state.infoBetData,
                    fill: true,
                    borderColor: 'red',
                    backgroundColor: 'rgba(179,3,3,0.29)'
                }
            ]
        };

        const options = {
            title: {
                display: true,
                text: 'Bets per day',
                fontSize: 20,
                fontColor: 'black'
            },
            legend: {
                position: 'bottom',
                fontSize: 20
            },
        };

        const data2 = {
            labels: this.state.labelsDataUsers,
            datasets: [
                {
                    label: 'Quantity of users per day',
                    data: this.state.infoUserData,
                    fill: true,
                    borderColor: 'rgba(13,251,132,1.00)',
                    backgroundColor: 'rgba(113,251,132,0.27)',
                }
            ]
        };

        const options2 = {
            title: {
                display: true,
                text: 'User registration per day',
                fontSize: 20,
                fontColor: 'black'
            },
            legend: {
                position: 'bottom',
                fontSize: 20,
            },
        };

        return (
            <Fragment>
                <div className='UserChartView'>
                   <TabView>
                       <TabPanel leftIcon="pi pi-user-plus" header="User registration per day">
                           <Chart type="line" options={options2} data={data2}/>
                       </TabPanel>
                       <TabPanel leftIcon="pi pi-chart-line" header="Bets per day">
                           <Chart type="line" options={options} data={data}/>
                       </TabPanel>
                   </TabView>
                </div>
            </Fragment>
        );
    }

    componentDidMount() {
        this.getLabelsBet();
        this.getDataBets();
        this.getLabelsUsers();
        this.getDataUsers();
    }

    getLabelsBet = () => {
        axios.get('https://localhost:44355/api/Apuestas?dateId=1').then((resultData) => {
            this.setState({labelsData: resultData.data}, () => {
                console.log(this.state.labelsData);
            });

        });
    }

    getLabelsUsers = () => {
        axios.get('https://localhost:44355/api/Usuarios?dateId=1').then((resultData) => {
            this.setState({labelsDataUsers: resultData.data}, () => {
                console.log(this.state.labelsDataUsers);
            });

        });
    }

    getDataBets = () => {
        axios.get('https://localhost:44355/api/Apuestas?quantityBet=1').then((resultData) => {
            this.setState({infoBetData: resultData.data}, () => {
                console.log(this.state.infoBetData);
            });

        });
    }

    getDataUsers = () => {
        axios.get('https://localhost:44355/api/Usuarios?countUsers=1').then((resultData) => {
            this.setState({infoUserData: resultData.data}, () => {
                console.log(this.state.infoUserData);
            });

        });
    }
}

