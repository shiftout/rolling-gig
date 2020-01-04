const uri = 'https://localhost:44307/api/';
let todos = [];

function getItems() {
    fetch(uri + 'todoitems')
        .then(response => response.json())
        .then(data => _displayTodoItems(data))
        .catch(error => console.error('Unable to get items.', error));

    fetch(uri + 'tags')
        .then(response => response.json())
        // .then(data => _displayTags(data))
        .then(data => _displayCount('tags-counter', data.length))
        .catch(error => console.error('Unable to get tags.', error));        
}

function addItem() {
    const addNameTextbox = document.getElementById('add-todo-title');

    const item = {
        isComplete: false,
        title: addNameTextbox.value.trim()
    };

    fetch(uri, {
        method: 'POST',
        headers: {
            'Accept': 'application/json',
            'Content-Type': 'application/json'
        },
        body: JSON.stringify(item)
    })
        .then(response => response.json())
        .then(() => {
            getItems();
            addNameTextbox.value = '';
        })
        .catch(error => console.error('Unable to add item.', error));
}

function deleteItem(id) {
    fetch(`${uri}/${id}`, {
        method: 'DELETE'
    })
        .then(() => getItems())
        .catch(error => console.error('Unable to delete item.', error));
}

function displayEditForm(id) {
    const item = todos.find(item => item.id === id);

    document.getElementById('edit-title').value = item.title;
    document.getElementById('edit-id').value = item.id;
    document.getElementById('edit-isComplete').checked = item.isComplete;
    document.getElementById('editForm').style.display = 'block';
}

function updateItem() {
    const itemId = document.getElementById('edit-id').value;
    const item = {
        id: parseInt(itemId, 10),
        isComplete: document.getElementById('edit-isComplete').checked,
        title: document.getElementById('edit-title').value.trim()
    };

    fetch(`${uri}/${itemId}`, {
        method: 'PUT',
        headers: {
            'Accept': 'application/json',
            'Content-Type': 'application/json'
        },
        body: JSON.stringify(item)
    })
        .then(() => getItems())
        .catch(error => console.error('Unable to update item.', error));

    closeInput();

    return false;
}

function closeInput() {
    document.getElementById('editForm').style.display = 'none';
}

function _displayCount(elementId, itemCount) {
    document.getElementById(elementId).innerText = `${itemCount}`;
}

function _displayTodoItems(data) {
    _displayCount('todos-counter', data.length);
    
    const container = document.getElementById('todos-container');
    container.classList.add('card-columns');
    container.innerHTML = '';

    // alert(JSON.stringify(data));

    const button = document.createElement('button');
    button.classList.add('btn', 'btn-sm', 'btn-secondary');

    data.forEach(item => {
        let card = document.createElement('div');
        card.classList.add('card');

        let tags = (item.tags.length > 0) ? 'tags: ' + item.tags.map(t => t.name).join(',') : 'no tags';

        card.innerHTML = `
        <div class="card-body">
            <h5 class="card-title">${item.title}</h5>
            <h6 class="card-subtitle mb-2 text-muted">#${item.id}, ${tags}</h6>
            <p class="card-text"></p>
            <a href="#" class="btn btn-outline-success">Close</a>
            <a href="#" class="btn btn-outline-danger">Delete</a>
        </div>
        <div class="card-footer text-muted">
            Last modified: ${item.lastModified} 
        </div>
        `;
        container.appendChild(card);
        linebreak = document.createElement('br');
        container.appendChild(linebreak);
    });

}
