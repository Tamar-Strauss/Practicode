import axios from 'axios';

const apiUrl = process.env.REACT_APP_API;
axios.defaults.baseURL = apiUrl;

function saveAccessToken(authResult) {
  localStorage.setItem('access_token', authResult.token);
  setAuthorizationBearer();
}

function setAuthorizationBearer() {
  const accessToken = localStorage.getItem('access_token');
  if (accessToken)
    axios.defaults.headers.common['Authorization'] = `Bearer ${accessToken}`;
}

axios.interceptors.response.use(
  function (res) {
    return res.data
  },
  function (err) {
    console.log(err);
    if (err.response.status === 401) {
      return (window.location.href = '/login')
    }
  });

export default {
  // Functions
  getTasks: async () => {
    const result = await axios.get(`${apiUrl}/items`).catch(err => err);
    return result;
  },

  addTask: async (name) => {
    console.log('addTask', name)
    const obj = {
      Name: name,
      IsComplete: false
    }
    const result = await axios.post(`${apiUrl}/items`, obj).catch(err => err);
    return result;
  },

  setCompleted: async (id, isComplete) => {
    const task = {
      id: id,
      isComplete: isComplete
    }
    const result = await axios.put(`${apiUrl}/items/${id}`, task).catch(err => err);
    return result;
  },

  deleteTask: async (id) => {
    const result = await axios.delete(`${apiUrl}/items/${id}`).catch(err => err);
    return result;
  }
};
