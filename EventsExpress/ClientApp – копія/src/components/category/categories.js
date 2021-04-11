import React, { Component } from 'react';
import CategoryAddWrapper from '../../containers/categories/category-add';
import CategoryListWrapper from '../../containers/categories/category-list';
import Spinner from '../spinner';

import get_categories from '../../actions/category/category-list';

import { connect } from 'react-redux';
class Categories extends Component{
    
    
    componentWillMount = () => this.props.get_categories();

    render() {
        const { isPending, data} = this.props.categories;
        return <div>
                <table className="table w-75 m-auto">
                    <tbody>
                        <CategoryAddWrapper 
                            item={{name: "", id: "00000000-0000-0000-0000-000000000000"}} 
                        />
                        {!isPending ? <CategoryListWrapper data={data} /> : null }
                    </tbody>
                </table> 
                {isPending ? <Spinner/> : null}
            </div>
    }
}

const mapStateToProps = (state) => ({categories: state.categories});


const mapDispatchToProps = (dispatch) => {
    return {
        get_categories: () => dispatch(get_categories())
    }
};


export default connect(mapStateToProps, mapDispatchToProps)(Categories);