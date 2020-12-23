const initState = {
    posts: [],
    firstInit: true
  }
  const rootReducer = (state = initState, action) => {
    // console.log(action) // show in console
    // // let's delete it from the state
    // if (action.type == 'DELETE_POST') {
    //   let new_posts = state.posts.filter(p => p.id != action.id)
    //   return {
    //     ...state,
    //     posts: new_posts
    //   }
    // }
    // else if (action.type == 'AJAX_POSTS') {
    //   console.log(action.data)
    //   if (state.firstInit) {
    //     state.firstInit = false
    //     return {
    //       ...state,
    //       posts: action.data
    //     }
    //   }
    // }
  
    return state;
  
  };
  
  export default rootReducer  