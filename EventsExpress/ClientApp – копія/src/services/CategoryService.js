import EventsExpressService from './EventsExpressService'

const baseService = new EventsExpressService();

export default class CategoryService {

    getAllCategories = async () => {
        const res = await baseService.getResource('category/all');
        return res;
    }

    setCategory = async (data) => {
        const res = await baseService.setResource('category/create', {
            name: data.name
        });
        return !res.ok
            ? { error: await res.text() }
            : res;
    }

    editCategory = async (data) => {
        const res = await baseService.setResource('category/edit', {
            id: data.id,
            name: data.name
        });
        return !res.ok
            ? { error: await res.text() }
            : res;
    }

    setCategoryDelete = async (data) => {
        const res = await baseService.setResource(`category/delete/${data}`);
        return !res.ok
            ? { error: await res.text() }
            : res;
    }
}