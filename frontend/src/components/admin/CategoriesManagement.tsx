import { useState, useEffect } from 'react';
import {
  Box,
  Button,
  TextField,
  List,
  ListItem,
  ListItemText,
  ListItemSecondaryAction,
  IconButton,
  Dialog,
  DialogTitle,
  DialogContent,
  DialogActions,
} from '@mui/material';
import { Delete as DeleteIcon, Edit as EditIcon } from '@mui/icons-material';
import api from '../../api/axios';

interface Category {
  id: string;
  title: string;
}

export const CategoriesManagement = () => {
  const [categories, setCategories] = useState<Category[]>([]);
  const [open, setOpen] = useState(false);
  const [editingCategory, setEditingCategory] = useState<Category | null>(null);
  const [categoryName, setCategoryName] = useState('');
  const [categoryDescription, setCategoryDescription] = useState('');

  const fetchCategories = async () => {
    try {
      const response = await api.get('/category/all');
      setCategories(response.data);
    } catch (error) {
      console.error('Ошибка при загрузке категорий:', error);
    }
  };

  useEffect(() => {
    fetchCategories();
  }, []);

  const handleOpen = (category?: Category) => {
    if (category) {
      setEditingCategory(category);
      setCategoryName(category.title);
    } else {
      setEditingCategory(null);
      setCategoryName('');
      setCategoryDescription('');
    }
    setOpen(true);
  };

  const handleClose = () => {
    setOpen(false);
    setEditingCategory(null);
    setCategoryName('');
    setCategoryDescription('');
  };


  const handleSubmit = async () => {
    try {
      if (editingCategory) {
        await api.put(`/categories/${editingCategory.id}`, {
          name: categoryName,
          description: categoryDescription,
        });
      } else {
        await api.post('/category/create', {
          title: categoryName
        });
      }
      handleClose();
      fetchCategories();
    } catch (error) {
      console.error('Ошибка при сохранении категории:', error);
    }
  };

  const handleDelete = async (id: string) => {
    try {
      await api.delete(`/categories/${id}`);
      fetchCategories();
    } catch (error) {
      console.error('Ошибка при удалении категории:', error);
    }
  };

  return (
    <Box>
      <Box sx={{ mb: 2 }}>
        <Button variant="contained" onClick={() => handleOpen()}>
          Добавить категорию
        </Button>
      </Box>

      <List>
        {categories.map((category) => (
          <ListItem key={category.id}>
            <ListItemText
              primary={category.title}
            />
            <ListItemSecondaryAction>
              <IconButton edge="end" onClick={() => handleOpen(category)}>
                <EditIcon />
              </IconButton>
              <IconButton edge="end" onClick={() => handleDelete(category.id)}>
                <DeleteIcon />
              </IconButton>
            </ListItemSecondaryAction>
          </ListItem>
        ))}
      </List>

      <Dialog open={open} onClose={handleClose}>
        <DialogTitle>
          {editingCategory ? 'Редактировать категорию' : 'Добавить категорию'}
        </DialogTitle>
        <DialogContent>
          <TextField
            autoFocus
            margin="dense"
            label="Название категории"
            fullWidth
            value={categoryName}
            onChange={(e) => setCategoryName(e.target.value)}
          />
        </DialogContent>
        <DialogActions>
          <Button onClick={handleClose}>Отмена</Button>
          <Button onClick={handleSubmit} variant="contained">
            {editingCategory ? 'Сохранить' : 'Добавить'}
          </Button>
        </DialogActions>
      </Dialog>
    </Box>
  );
}; 